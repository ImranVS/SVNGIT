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
                                         Type = x.Type,
                                         Country = x.Location,
                                         Name = x.Name,
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
                Response = Common.CreateResponse(result);
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
                                         Type = x.Type,
                                         Country = x.Location,
                                         Name = x.Name,
                                         Version = x.SoftwareVersion,
                                         LastUpdated = x.LastUpdated,
                                         Description = x.Description,
                                         Status = x.StatusCode,
                                         DeviceId = x.DeviceId


                                     })).FirstOrDefault();
                var serviceIcons = Common.GetServerTypeIcons();
                Models.ServerType serverType = Common.GetServerTypeTabs(result.Type);
                result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper()).ToList();
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
                                List<Segment> segments = new List<Segment>();
                               
                                    var result = statsHourly
                                           .GroupBy(row => new
                                           {
                                               row.CreatedOn.Hour,
                                               row.StatName

                                           })
                                           .Select(row => new
                                           {
                                               Hour = row.Key.Hour,
                                               Value = row.Average(x => x.StatValue),
                                               StatName = row.Key.StatName

                                           }).ToList();

                                DateTime time = new DateTime();
                                string displayTime = "";
                               // List<string> lstinternalcounter = result.Select(c => c.()).ToList();
                                List<double> values = new List<double>();
                                // int onhour = moment.Hour;
                                foreach (string name in statNames)
                                {
                                    for (int hour = 1; hour <= 24; hour++)
                                    {
                                        var item = result.Where(x => x.Hour == hour && x.StatName==name).ToList();
                                        var statdata = result.Where(x => x.Hour == hour).FirstOrDefault();
                                        var output = result.Where(x=>x.StatName==name).Select(x => x.Value).ToList();

                                       
                                            if (statdata != null && statNames.Length==1)
                                            {

                                                time = DateTime.Today.AddDays(-1).AddHours(hour);
                                                displayTime = time.ToString("hh:mm tt");
                                                segments.Add(new Segment { Label = displayTime.ToString(), Value = statdata.Value, StatName = statdata.StatName });


                                            }
                                        
                                        
                                        else if (item != null && statNames.Length>1)
                                            {
                                                time = DateTime.Today.AddDays(-1).AddHours(hour);
                                                displayTime += time.ToString("hh:mm tt")+",";
                                                values = output.ToList();
                                            }
                                        

                                        else
                                        {
                                            time = DateTime.Today.AddHours(hour);
                                            displayTime = time.ToString("hh:mm tt");
                                            segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });

                                        }


                                    }
                                    // displayTime = displayTime.ToList<string>();
                                    List<string> timevalue = new List<string>();
                                    timevalue.Add(displayTime);
                                    if (statNames.Length > 1)
                                    {
                                        segments.Add(new Segment { Time = timevalue.ToList(), StatName = name, Statvalues = values.ToList() });
                                    }
                                  
                                }
                                    Serie serie = new Serie();
                                    serie.Title = statName;
                                    serie.Segments = segments;

                                    List<Serie> series = new List<Serie>();
                                    series.Add(serie);

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

            try
            {
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<SummaryStatistics, bool>> expression = (p => p.StatName == statName);
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

                    Expression<Func<SummaryStatistics, bool>> expression = (p => p.StatName == statName && p.DeviceId == deviceId);
                    var statsHourly = summaryRepository.Find(expression);
                    var result = statsHourly
                                .GroupBy(row => row.CreatedOn.Date)
                                .Select(grp => new
                                {
                                    Date = grp.Key,
                                    Value = grp.Average(x => x.StatValue)
                                }).ToList();
                    List<Segment> segments = new List<Segment>();

                    DateTime now = DateTime.Now.AddDays(-28);
                    var startDate = new DateTime(now.Year, now.Month, 1);
                    var endDate = startDate.AddMonths(1).AddDays(-1);

                    for (DateTime date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1))
                    {
                        var item = result.Where(x => x.Date == date).FirstOrDefault();
                        if (item != null)
                        {
                            string y = date.ToString("d-MMMM-yyyy", CultureInfo.InvariantCulture);
                            segments.Add(new Segment { Label = y.ToString(), Value = item.Value });
                        }
                        else
                        {
                            string y = date.ToString("d-MMMM-yyyy", CultureInfo.InvariantCulture);
                            segments.Add(new Segment { Label = y.ToString(), Value = 0 });

                        }

                    }

                    Serie serie = new Serie();
                    serie.Title = statName;
                    serie.Segments = segments;

                    List<Serie> series = new List<Serie>();
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = statName;
                    chart.Series = series;



                    Response = Common.CreateResponse(chart);
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
                                         Name = x.Name,
                                         Status = x.StatusCode,
                                         Country = x.Location,
                                         Details = x.Details,
                                         UserCount = x.UserCount,
                                         CPU = x.CPU,
                                         Type = x.Type
                                         // LastUpdated = x.LastUpdated,
                                         // Description = x.Description,


                                     });
                    Response = Common.CreateResponse(result);

                }
                else if (!string.IsNullOrEmpty(type))
                {

                    Expression<Func<Status, bool>> expression = (p => p.Type == type);
                    var result = statusRepository.Find(expression).AsQueryable()
                                                                    .Select(x => new ServerStatus
                                                                    {

                                                                        // Id = x.Id,
                                                                        DeviceId = x.DeviceId,
                                                                        Name = x.Name,
                                                                        Status = x.StatusCode,
                                                                        Country = x.Location,
                                                                        Details = x.Details,
                                                                        UserCount = x.UserCount,
                                                                        CPU = x.CPU,
                                                                        Type = x.Type
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





    }
}
