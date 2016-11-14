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
using System.Dynamic;

namespace VitalSigns.API.Controllers
{

    [Route("[controller]")]
    public class ServicesController : BaseController
    {

        private IRepository<Server> serverRepository;
        private IRepository<Status> statusRepository;
        private IRepository<DailyStatistics> dailyRepository;
        private IRepository<SummaryStatistics> summaryRepository;
        private IRepository<NameValue> nameValueRepository;
        // private IRepository<DominoSettingsModel> dominoSettingsRepository;

        private string DateFormat = "yyyy-MM-dd";

        //private IRepository<IbmConnectionsTopStats> ibmRepository;




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
            var issue = result.Where(item => item.Label == "Issue").Select(x => x.Value);
            var ok = result.Where(item => item.Label == "OK").Select(x => x.Value);
            var notResponding = result.Where(item => item.Label == "Not Responding").Select(x => x.Value);
            var maintenance = result.Where(item => item.Label == "Maintenance").Select(x => x.Value);
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
                                         Status = x.StatusCode,// Holds the formated status code for displaying colors in UI
                                         StatusCode=x.StatusCode,//Holds actual server code data
                                         DeviceId = x.DeviceId,
                                         Location=x.Location

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
                    else
                        item.Status = string.Empty;
                    if (string.IsNullOrEmpty(item.Type))
                        item.Type = string.Empty;
                    if (string.IsNullOrEmpty(item.StatusCode))
                        item.StatusCode = string.Empty;

                    if (string.IsNullOrEmpty(item.Location))
                        item.Location = string.Empty;
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
        public APIResponse GetServerDetails(string device_id, string destination, string secondaryRole, string deviceType)
        {

            statusRepository = new Repository<Status>(ConnectionString);


            try
            {
                if (!string.IsNullOrEmpty(device_id))
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
                        var secondaryRoles = result.SecondaryRole.Split(';').Select(x => x.Trim());
                    result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper() && (x.SecondaryRole == null || secondaryRoles.Contains(x.SecondaryRole))).ToList();
                }


                result.Description = "Last Updated: " + result.LastUpdated.Value;
                result.Icon = serverType.Icon;
                if (!string.IsNullOrEmpty(result.Status))
                    result.Status = result.Status.ToLower().Replace(" ", "");
                Response = Common.CreateResponse(result);
            }
                else
                {
                    if (!string.IsNullOrEmpty(deviceType))
                    {
                        var result = (statusRepository.Collection.AsQueryable()
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

                        Models.ServerType serverType = Common.GetServerTypeTabs(deviceType);
                        result.Tabs = serverType.Tabs;
                        Response = Common.CreateResponse(result);
                    }
                }
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
        //[HttpGet("statistics")]
        //public APIResponse GetDailyStat(string deviceId, string statName, string operation)
        //{

        //    dailyRepository = new Repository<DailyStatistics>(ConnectionString);

        //    try
        //    {
        //        var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
        //        if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
        //        {


        //            Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName));
        //            var result = dailyRepository.Find(expression).Select(x => new StatsData
        //            {
        //                DeviceId = x.DeviceId,
        //                StatName = x.StatName,
        //                StatValue = x.StatValue

        //            }).OrderBy(x => x.StatName).ToList();


        //            Response = Common.CreateResponse(result);

        //        }
        //        else
        //        {
        //            Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName) && p.DeviceId == deviceId);

        //            if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
        //            {



        //                var result = dailyRepository.Find(expression).Select(x => new StatsData
        //                {
        //                    DeviceId = x.DeviceId,
        //                    StatName = x.StatName,
        //                    StatValue = x.StatValue

        //                }).OrderBy(x => x.StatName).ToList();

        //                Response = Common.CreateResponse(result);

        //                return Response;
        //            }
        //            else if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
        //            {

        //                // StatsData statsData;


        //                switch (operation.ToUpper())
        //                {
        //                    case "SUM":

        //                        var statsSum = dailyRepository.Find(expression);

        //                        var statsSumData = statsSum
        //                                      .GroupBy(row => row.StatName)
        //                                      .Select(grp => new
        //                                      {
        //                                          StatName = grp.Key,
        //                                          Value = grp.Sum(x => x.StatValue),
        //                                          DeviceId = deviceId
        //                                      }).ToList();

        //                        // statsData = new StatsData { StatName = statName, StatValue = statsSum, DeviceId = deviceId };
        //                        Response = Common.CreateResponse(statsSumData);

        //                        break;

        //                    case "AVG":

        //                        var statsAvg = dailyRepository.Find(expression);

        //                        var statsAvgData = statsAvg
        //                                  .GroupBy(row => row.StatName)
        //                                  .Select(grp => new
        //                                  {
        //                                      StatName = grp.Key,
        //                                      Value = grp.Average(x => x.StatValue),
        //                                      DeviceId = deviceId
        //                                  }).ToList();
        //                        Response = Common.CreateResponse(statsAvgData);
        //                        break;
        //                    case "COUNT":
        //                        var statsCount = dailyRepository.Find(expression);

        //                        var statsCountData = statsCount
        //                               .GroupBy(row => row.StatName)
        //                               .Select(grp => new
        //                               {
        //                                   StatName = grp.Key,
        //                                   Value = grp.Count(),
        //                                   DeviceId = deviceId
        //                               }).ToList();
        //                        Response = Common.CreateResponse(statsCountData);
        //                        break;
        //                    case "HOURLY":
        //                        var statsHourly = dailyRepository.Find(expression);


        //                        var result = statsHourly
        //                               .GroupBy(row => new
        //                               {
        //                                   row.CreatedOn.Hour,
        //                                   row.StatName

        //                               })
        //                               .Select(row => new
        //                               {
        //                                   Hour = row.Key.Hour,
        //                                   Value = Math.Round(row.Average(x => x.StatValue), 2),
        //                                   StatName = row.Key.StatName

        //                               }).ToList();






        //                        // DateTime moment = DateTime.Now.Hour;
        //                        // int onhour = moment.Hour;
        //                        List<Serie> series = new List<Serie>();

        //                        foreach (var name in statNames)
        //                        {

        //                            List<Segment> segments = new List<Segment>();
        //                            Serie serie = new Serie();
        //                            for (int hour = 1; hour <= 24; hour++)
        //                            {
        //                                // To do
        //                                // string hourString =hour<12?hour.ToString()+ " A.M " 

        //                                var item = result.Where(x => x.Hour == hour).FirstOrDefault();
        //                                var output = result.Where(x => x.Hour == hour && x.StatName == name).ToList();
        //                                DateTime time = new DateTime();
        //                                time = DateTime.Now.AddHours(-hour);
        //                                time = time.AddMinutes(-1 * time.Minute);
        //                                string displayTime = "";
        //                                displayTime = time.ToString("hh:mm tt");


        //                                if (item != null && statNames.Length == 1)
        //                                {

        //                                    segments.Add(new Segment { Label = displayTime.ToString(), Value = item.Value });
        //                                    serie.Title = name;
        //                                    serie.Segments = segments;
        //                                }
        //                                else if (item != null && output != null && statNames.Length > 1)
        //                                {
        //                                    foreach (var statvalue in output)
        //                                    {
        //                                        segments.Add(new Segment { Label = displayTime, Value = statvalue.Value });
        //                                        serie.Title = name;
        //                                        serie.Segments = segments;
        //                                    }

        //                                }
        //                                else
        //                                {

        //                                    // TimeSpan timespan = new TimeSpan(hour);

        //                                    segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });
        //                                    serie.Title = name;
        //                                    serie.Segments = segments;

        //                                }





        //                            }
        //                            series.Add(serie);
        //                        }

        //                        Chart chart = new Chart();
        //                        chart.Title = statName;
        //                        chart.Series = series;



        //                        Response = Common.CreateResponse(chart);
        //                        break;
        //                }


        //            }
        //        }
        //        return Response;



        //    }
        //    catch (Exception exception)
        //    {
        //        Response = Common.CreateResponse(null, "Error", exception.Message);

        //        return Response;
        //    }
        //}



        [HttpGet("statistics")]
        public APIResponse GetDailyStat(string deviceId, string statName, string operation, bool isChart = false)
        {

            dailyRepository = new Repository<DailyStatistics>(ConnectionString);

            try
            {
                var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => statNames.Contains(p.StatName));
                    if (string.IsNullOrEmpty(operation))
                    {
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
                        List<Serie> series = new List<Serie>();
                        List<Segment> segments = new List<Segment>();
                        Serie serie = new Serie();
                        Chart chart = new Chart();

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
                                Response = Common.CreateResponse(statsSumData);
                                break;

                            case "AVG":

                                var statsAvg = dailyRepository.Find(expression);

                                if (isChart)
                                {
                                    var statsAvgData = statsAvg
                                          .GroupBy(row => row.DeviceName)
                                          .Select(grp => new
                                          {
                                              Label = grp.Key,
                                              Value = grp.Average(x => x.StatValue)
                                          }).ToList();
                                    foreach (var item in statsAvgData)
                                    {
                                        segments.Add(new Segment()
                                        {
                                            Label = item.Label,
                                            Value = item.Value
                                        });
                                    }

                                serie = new Serie();
                                serie.Title = "test";
                                serie.Segments = segments;
                                series.Add(serie);

                                chart = new Chart();
                                chart.Title = "";
                                chart.Series = series;
                                Response = Common.CreateResponse(chart);
                                }
                                else
                                {
                                    var statsAvgData = statsAvg
                                          .GroupBy(row => row.DeviceName)
                                          .Select(grp => new
                                          {
                                              StatName = grp.Key,
                                              Value = grp.Sum(x => x.StatValue),
                                              DeviceId = deviceId
                                          }).ToList();
                                    Response = Common.CreateResponse(statsAvgData);
                                }
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

                                foreach (var name in statNames)
                                {

                                    segments = new List<Segment>();
                                    serie = new Serie();
                                    for (int hour = 1; hour <= 24; hour++)
                                    {
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
                                            segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });
                                            serie.Title = name;
                                            serie.Segments = segments;
                                        }
                                    }
                                    series.Add(serie);
                                }

                                chart = new Chart();
                                chart.Title = statName;
                                chart.Series = series;
                                Response = Common.CreateResponse(chart);
                                break;
                        }
                    }
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
        public APIResponse GetSummaryStat(string deviceId, string statName, string seriesTitle = "", string startDate = "", string endDate = "", bool isChart = true)
        {
            //DateFormat is YYYY-MM-DD
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
            var statNames = statName.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
            try
            {
                FilterDefinition<SummaryStatistics> filterDefTemp;
                FilterDefinition<SummaryStatistics> filterDef = summaryRepository.Filter.Gte(p => p.CreatedOn, dtStart) &
                    summaryRepository.Filter.Lte(p => p.CreatedOn, dtEnd);

                if (!string.IsNullOrEmpty(statName) && isChart == false)
                {
                    if (string.IsNullOrEmpty(deviceId))
                    {
                        filterDefTemp = filterDef &
                        summaryRepository.Filter.And(summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*")))),
                                                     summaryRepository.Filter.Ne(p => p.DeviceName, null));
                    }
                    else
                    {
                        filterDefTemp = filterDef &
                        summaryRepository.Filter.And(summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*")))),
                                                     summaryRepository.Filter.Ne(p => p.DeviceName, null),
                                                     summaryRepository.Filter.Eq(p => p.DeviceId, deviceId));

                    }

                    var result = summaryRepository.Find(filterDefTemp).Select(x => new StatsData
                    {
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName,
                        StatName = x.StatName,
                        StatValue = x.StatValue

                    }).Take(500).ToList();

                    foreach (string currString in statNames.Where(i => i.Contains("*")))
                    {
                        filterDefTemp = filterDef &
                            summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression(currString.Replace("*", ".*"), "i"));

                        if (!string.IsNullOrEmpty(deviceId))
                        {
                            filterDefTemp = filterDefTemp & summaryRepository.Filter.Eq(p => p.DeviceId, deviceId);
                        }


                        result.AddRange(
                            summaryRepository.Find(filterDefTemp).Select(x => new StatsData
                            {
                                DeviceId = x.DeviceId,
                                DeviceName = x.DeviceName,
                                StatName = x.StatName,
                                StatValue = x.StatValue

                            }).Take(500).ToList()
                        );

                    }
                    Response = Common.CreateResponse(result);

                }
                else if (!string.IsNullOrEmpty(statName))
                {
                    if (!string.IsNullOrEmpty(deviceId))
                    {
                        filterDef = filterDef &
                        summaryRepository.Filter.Eq(p => p.DeviceId, deviceId);
                    }
                    filterDefTemp = filterDef &
                        summaryRepository.Filter.In(p => p.StatName, statNames.Where(i => !(i.Contains("*"))));


                    var statsHourly = summaryRepository.Find(filterDefTemp);

                    var result = statsHourly
                                       .GroupBy(row => new
                                       {
                                           row.CreatedOn.Date,
                                           row.StatName,
                                           row.DeviceName

                                       })
                                       .Select(row => new
                                       {
                                           Date = row.Key.Date,
                                           Value = Math.Round(row.Average(x => x.StatValue), 2),
                                           StatName = row.Key.StatName,
                                           DeviceName = row.Key.DeviceName

                                       }).ToList();

                    foreach (string currString in statNames.Where(i => i.Contains("*")))
                    {
                        filterDefTemp = filterDef & 
                            summaryRepository.Filter.Regex(p => p.StatName, new BsonRegularExpression(currString.Replace("*", ".*"), "i"));

                        result.AddRange(
                            summaryRepository.Find(filterDefTemp)
                            .GroupBy(row => new
                            {
                                row.CreatedOn.Date,
                                row.StatName,
                                row.DeviceName
                            })
                            .Select(row => new 
                            {
                                Date = row.Key.Date,
                                Value = Math.Round(row.Average(x => x.StatValue), 2),
                                StatName = row.Key.StatName,
                                DeviceName = row.Key.DeviceName

                            }).Take(500).ToList()
                        );
                    }

                    List<Serie> series = new List<Serie>();

                    if ((dtEnd - dtStart).TotalDays <= 1)
                    {
                        var segments = new List<Segment>();

                        foreach (var item in result)
                        {
                            segments.Add(new Segment()
                            {
                                Label = item.StatName,
                                Value = item.Value
                            });
                        }

                        Serie serie = new Serie();
                        serie.Title = dtStart.ToString(DateFormat);
                        serie.Segments = segments;

                        series.Add(serie);
                        
                    }
                    else
                    {
                        foreach (var name in result.Select(i => i.StatName).Distinct())
                        {

                            List<Segment> segments = new List<Segment>();
                            Serie serie = new Serie();
                            var devicename = result.Where(x => x.StatName == name.ToString()).ToList();

                            //WS changed to just less then end date due to the end date being the next day to include all of the previous day values.
                            for (DateTime date = dtStart.Date; date.Date < dtEnd.Date; date = date.AddDays(1))
                            {
                                var item = result.Where(x => x.Date == date && x.StatName == name.ToString()).FirstOrDefault();
                                var output = result.Where(x => x.Date == date && x.StatName == name.ToString()).ToList();
                                
                                string statdate = date.ToString("d-MMMM-yyyy", CultureInfo.InvariantCulture);
                                if (item != null && statNames.Length == 1)
                                {
                                    segments.Add(new Segment { Label = statdate.ToString(), Value = item.Value });
                                    if (string.IsNullOrEmpty(seriesTitle))
                                    {
                                        serie.Title = name.ToString(); 
                                    }
                                    else
                                    {
                                        serie.Title = devicename[0].DeviceName;
                                    }
                                    serie.Segments = segments;
                                }
                                else if (item != null && output != null && statNames.Length > 1)
                                {
                                    foreach (var statvalue in output)
                                    {
                                        segments.Add(new Segment { Label = statdate, Value = statvalue.Value });
                                        if (string.IsNullOrEmpty(seriesTitle))
                                        {
                                            serie.Title = name.ToString();
                                        }
                                        else
                                        {
                                            serie.Title = statvalue.DeviceName;
                                        }
                                        serie.Segments = segments;
                                    }
                                }
                                else
                                {
                                    segments.Add(new Segment { Label = statdate.ToString(), Value = 0 });
                                    if (string.IsNullOrEmpty(seriesTitle))
                                    {
                                        serie.Title = name.ToString();
                                    }
                                    else
                                    {
                                        serie.Title = devicename[0].DeviceName;
                                    }
                                    serie.Segments = segments;
                                }
                                
                            }
                            series.Add(serie);

                        }
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

        //[HttpGet("top_tags")]
        //public APIResponse GetIbmConnection(string deviceId)
        //{
        //    ibmRepository = new Repository<IbmConnectionsTopStats>(ConnectionString);
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(deviceId))
        //        {

        //            Expression<Func<IbmConnectionsTopStats, bool>> expression = (p => p.DeviceId == deviceId);
        //            var topTags = ibmRepository.Find(expression);
        //            var result = topTags
        //                              .GroupBy(row => row.Name)
        //                              .Select(grp => new Segment
        //                              {
        //                                  Label = grp.Key,
        //                                  Value = grp.Count(),

        //                              }).ToList();
        //            List<Segment> segments = new List<Segment>();
        //            Serie serie = new Serie();
        //            //serie.Title = name;
        //            serie.Segments = result.OrderBy(x => x.Value).Take(5).ToList();

        //            List<Serie> series = new List<Serie>();
        //            series.Add(serie);

        //            Chart chart = new Chart();
        //            // chart.Title = name;
        //            chart.Series = series;
        //            Response = Common.CreateResponse(chart);

        //        }
        //        return Response;
        //    }
        //    catch (Exception exception)
        //    {
        //        Response = Common.CreateResponse(null, "Error", exception.Message);

        //        return Response;
        //    }
        //}


        [HttpGet("status_list")]
        public APIResponse GetStatusList(string type, string docfield = "", string sortby = "", bool isChart = false)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            List<Status> statslist = null;
            Expression<Func<Status, bool>> expression;
            List<dynamic> result = new List<dynamic>();
            List<Segment> segments = new List<Segment>();
            List<Status> list = null;
            Segment segment = new Segment();

            try
            {
                if (string.IsNullOrEmpty(type))
                {
                    list = statusRepository.Collection.AsQueryable().OrderBy(x => x.DeviceName).ToList();

                    foreach (Status status in list)
                    {
                        var x = new ExpandoObject() as IDictionary<string, Object>;
                        foreach (var field in status.ToBsonDocument())
                        {
                            x.Add(field.Name, field.Value.ToString());
                        }
                        result.Add(x);
                    }
                    Response = Common.CreateResponse(result);
                }
                else if (!string.IsNullOrEmpty(type))
                {
                    if (string.IsNullOrEmpty(docfield))
                    {
                        if (type == "Traveler")
                        {
                            FilterDefinition<Status> filterDef = statusRepository.Filter.Exists(p => p.TravelerStatus);
                            list = statusRepository.Find(filterDef).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        else
                        {
                            expression = (p => p.DeviceType == type);
                            list = statusRepository.Find(expression).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        
                        foreach (Status status in list)
                        {
                            var x = new ExpandoObject() as IDictionary<string, Object>;
                            foreach (var field in status.ToBsonDocument())
                            {
                                x.Add(field.Name, field.Value.ToString());
                            }
                            result.Add(x);
                        }
                        Response = Common.CreateResponse(result);
                    }
                    else
                    {
                        if (type == "Traveler")
                        {
                            FilterDefinition<Status> filterDef = statusRepository.Filter.Exists(p => p.TravelerStatus);
                            statslist = statusRepository.Find(filterDef).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                        }
                        else
                        {
                            FilterDefinition<Status> filterdefStatus = statusRepository.Filter.Eq(x => x.DeviceType, type);
                            statslist = statusRepository.Collection.Find(filterdefStatus).ToList();
                        }
                        
                        if (!string.IsNullOrEmpty(sortby))
                        {
                            var propertyInfo = typeof(Status).GetProperty(sortby);
                            statslist = statslist.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        else
                        {
                            var propertyInfo = typeof(Status).GetProperty("DeviceName");
                            statslist = statslist.OrderBy(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (Status status in statslist)
                        {
                            var x = new ExpandoObject() as IDictionary<string, Object>;
                            var bson2 = status.ToBsonDocument();

                            if (bson2.Contains(docfield) && bson2.Contains("device_name"))
                            {
                                var statname = bson2["device_name"].ToString();
                                var statvalue = bson2[docfield].ToString();

                                if (!isChart)
                                {
                                    x.Add(statname, statvalue);
                                    result.Add(x);
                                }
                                else
                                {
                                    segment = new Segment();
                                    segment.Label = bson2["device_name"].ToString();
                                    segment.Value = bson2[docfield].ToDouble();
                                    segments.Add(segment);
                                }
                            }
                        }
                        if (!isChart)
                        {
                            Response = Common.CreateResponse(result);
                        }
                        else
                        {
                            Serie serie = new Serie();
                            serie.Title = "";
                            serie.Segments = segments;
                            List<Serie> series = new List<Serie>();
                            series.Add(serie);
                            Chart chart = new Chart();
                            chart.Series = series;
                            chart.Title = "";
                            Response = Common.CreateResponse(chart);
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

    [HttpGet("status_count")]
        public APIResponse GetStatusCount(string type, string docfield)
        {

            statusRepository = new Repository<Status>(ConnectionString);

            try
            {
                string color = "";
                var types = type.Replace("[", "").Replace("]", "").Replace(" ", "").Split(',');
                
                Expression<Func<Status, bool>> expression = (p => types.Contains(p.DeviceType));
                var bsonDocs = statusRepository.Collection.Aggregate()
                                    .Match(expression)
                                    .Group(new BsonDocument { { "_id", "$" + docfield }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                List<Segment> result = new List<Segment>();
                foreach (BsonDocument doc in bsonDocs)
                {
                    if (!doc["_id"].IsBsonNull)
                    {
                        if (doc["_id"].ToString() == "Not Responding")
                        {
                            color = "rgba(239, 58, 36, 1)";
                        }
                        else if (doc["_id"].ToString() == "OK")
                        {
                            color = "rgba(95, 190, 127, 1)";
                        }
                        else if (doc["_id"].ToString() == "Issue")
                        {
                            //color = "rgba(255, 195, 0, 1)";
                            color = "rgba(249, 156, 28, 1)";
                        }
                        else if (doc["_id"].ToString() == "Maintenance")
                        {
                            color = "rgba(119 , 119, 119, 1)";
                        }
                        Segment segment = new Segment()
                        {
                            //Might have to add additional types for support.  Format is IfThis ? DoThis : Else
                            Label = doc["_id"].ToString(),
                            Value = doc["count"].AsInt32,
                            Color = color
                        };
                        result.Add(segment);
                    }
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

                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

        }

        [HttpGet("disk_space")]
        public APIResponse GetDiskSpace(string deviceid = "")
        {
            FilterDefinition<Status> filterDefStatus;
            List<dynamic> result = new List<dynamic>();
            List<DiskStatus> disks = new List<DiskStatus>();
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                if (!string.IsNullOrEmpty(deviceid))
                {
                    filterDefStatus = statusRepository.Filter.And(statusRepository.Filter.Exists(x => x.Disks, true),
                        statusRepository.Filter.Eq(x => x.DeviceId, deviceid));
                }
                else
                {
                    filterDefStatus = statusRepository.Filter.Exists(x => x.Disks, true);
                }
                var result1 = statusRepository.Find(filterDefStatus).AsQueryable().OrderBy(x => x.DeviceName).ToList();
                List<Serie> diskserie = new List<Serie>();
                Serie diskFreeSerie = new Serie();
                diskFreeSerie.Title = "Available";
                Serie diskUsedSerie = new Serie();
                diskUsedSerie.Title = "Used";

                List<Segment> diskfreesegments = new List<Segment>();
                List<Segment> diskusedsegments = new List<Segment>();

                foreach (Status status in result1)
                {
                    disks = status.Disks;
                    disks.RemoveAll(item => item.DiskFree == null || item.DiskFree == 0.0);
                    disks.RemoveAll(item => item.DiskSize - item.DiskFree == null || item.DiskFree == 0.0);
                    var data = disks.Select(x => new
                    {
                        Name = x.DiskName,
                        Free = x.DiskFree,
                        Used = x.DiskSize - x.DiskFree
                    });

                    //diskFreeSerie.Segments.Add(data.Select(x => new Segment { Label = status.DeviceName + " - " + x.Name, Value = x.Free.Value, Color = "rgba(95, 190, 127, 1)" }).ToList());
                    foreach (var item in data)
                    {
                        diskfreesegments.Add(new Segment()
                        {
                            Label = status.DeviceName + " - " + item.Name,
                            Value = item.Free.Value,
                            Color = "rgba(95, 190, 127, 1)"
                        });
                        diskusedsegments.Add(new Segment()
                        {
                            Label = status.DeviceName + " - " + item.Name,
                            Value = item.Used.Value,
                            Color = "rgba(239, 58, 36, 1)"
                        });
                    }

                    //diskUsedSerie.Segments = data.Select(x => new Segment { Label = status.DeviceName + " - " + x.Name, Value = x.Used.Value, Color = "rgba(239, 58, 36, 1)" }).ToList();

                }
                diskFreeSerie.Segments = diskfreesegments;
                diskUsedSerie.Segments = diskusedsegments;
                diskserie.Add(diskFreeSerie);
                diskserie.Add(diskUsedSerie);
                Chart chart = new Chart();

                chart.Title = "Disk Space";
                chart.Series = diskserie;
                Response = Common.CreateResponse(chart);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("server_list_selectlist_data")]
        public APIResponse GetDeviceListDropDownData()
        {

            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                var deviceTypeData = statusRepository.All().Where(x=>x.DeviceType!=null).Select(x => x.DeviceType).Distinct().OrderBy(x=>x).ToList();
                var deviceStatusData = statusRepository.All().Where(x => x.StatusCode != null).Select(x => x.StatusCode).Distinct().OrderBy(x => x).ToList();
                var deviceLocationData = statusRepository.All().Where(x => x.Location != null).Select(x => x.Location).Distinct().OrderBy(x => x).ToList();
                deviceTypeData.Insert(0, "-All-");
                deviceStatusData.Insert(0, "-All-");
                deviceLocationData.Insert(0, "-All-");

                Response = Common.CreateResponse(new { deviceTypeData = deviceTypeData, deviceStatusData = deviceStatusData , deviceLocationData = deviceLocationData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("get-name-values")]
        public APIResponse GetNameVlaues(string category, string name)
        {
            try
            {

                nameValueRepository = new Repository<NameValue>(ConnectionString);
                if (string.IsNullOrEmpty(category) && string.IsNullOrEmpty(name))
                {
                    var result = nameValueRepository.All().ToList();
                    Response = Common.CreateResponse(result);

    }
                else if (!string.IsNullOrEmpty(category))
                {
                    Expression<Func<NameValue, bool>> expression = (p => p.Category == category);
                    var result = nameValueRepository.Find(expression)
                        .Select(x => new NameValueModel

                        { Name = x.Name, Id = x.Id, Category = x.Category, Value = x.Value }).ToList();
                    Response = Common.CreateResponse(result);
}
                else if (!string.IsNullOrEmpty(name))
                {
                    var names = name.Replace("[", "").Replace("]", "").Split(',');
                    Expression<Func<NameValue, bool>> expression = (p => names.Contains(p.Name));
                    var result = nameValueRepository.Find(expression).Select(x => new NameValueModel

                    { Name = x.Name, Id = x.Id, Category = x.Category, Value = x.Value }).ToList();
                    Response = Common.CreateResponse(result);

                }

               


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

            return Response;
        }

        [HttpGet("server_list_dropdown")]
        public APIResponse GetServersDropDown(string type)
        {

            try
            {
               serverRepository = new Repository<Server>(ConnectionString);
                var deviceNameData = serverRepository.All().Where(x => x.DeviceType == type).Select(x => x.DeviceName).Distinct().OrderBy(x => x).ToList();
                var result = serverRepository.All().Where(x => x.DeviceType == type).Select(x => new NameValueModel

                { Name = x.DeviceName, Id = x.Id }).ToList();
                Response = Common.CreateResponse(result);

                result.Insert(0, new NameValueModel { Name="", Id="" });

                Response = Common.CreateResponse(new { deviceNameData = result });
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




