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
                                         Status = x.StatusCode

                                     }).ToList();
                foreach (ServerStatus item in result)
                {

                    if (item.LastUpdated.HasValue)
                        item.Description = "Last Updated: " + item.LastUpdated.Value.ToShortDateString();
                    if (serviceIcons.ContainsKey(item.Type))
                        item.Icon = serviceIcons[item.Type];
                    else
                        item.Icon = @"/img/servers/Paintbrush.svg";
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
        public APIResponse GetServerDetails(string id, string destination, string secondaryRole)
        {

            statusRepository = new Repository<Status>(ConnectionString);


            try
            {
                Expression<Func<Status, bool>> expression = (p => p.Id == id);
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
                                         Status = x.StatusCode

                                     })).FirstOrDefault();
                var serviceIcons = Common.GetServerTypeIcons();
                Models.ServerType serverType = Common.GetServerTypeTabs(result.Type);
                result.Tabs = serverType.Tabs.Where(x => x.Type.ToUpper() == destination.ToUpper()).ToList();
                result.Description = "Last Updated: " + result.LastUpdated.Value.ToShortDateString();
                result.Icon = serverType.Icon;
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
                if (string.IsNullOrEmpty(deviceId) && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName);
                    var result = dailyRepository.Find(expression).Select(x => new StatsData
                    {
                      //DeviceId = x.DeviceId,
                        StatName = x.StatName,
                        StatValue = x.StatValue

                    }).Take(500).ToList();
                    Response = Common.CreateResponse(result);

                }
                else
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName && p.DeviceId == MongoDB.Bson.ObjectId.Parse(deviceId));

                    if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {

                        var result = dailyRepository.Find(expression).Select(x => new StatsData
                        {
                          // DeviceId = x.DeviceId,
                            StatName = x.StatName,
                            StatValue = x.StatValue

                        }).Take(5000).ToList();
                        Response = Common.CreateResponse(result);
                    }
                    else if (!string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {

                        StatsData statsData;
                        switch (operation.ToUpper())
                        {
                            case "SUM":

                                var statsSum = dailyRepository.Find(expression).Sum(x => x.StatValue);
                                statsData = new StatsData { StatName = statName, StatValue = statsSum, DeviceId = deviceId };
                                Response = Common.CreateResponse(statsSum);
                                break;
                            case "AVG":

                                var statsAvg = dailyRepository.Find(expression).Average(x => x.StatValue);
                                statsData = new StatsData { StatName = statName, StatValue = statsAvg, DeviceId = deviceId };
                                Response = Common.CreateResponse(statsAvg);
                                break;
                            case "COUNT":
                                var statsCount = dailyRepository.Find(expression).Count();
                                statsData = new StatsData { StatName = statName, StatValue = statsCount, DeviceId = deviceId };
                                Response = Common.CreateResponse(statsCount);
                                break;
                            case "HOURLY":
                                var statsHourly = dailyRepository.Find(expression);
                                var result = statsHourly
                                            .GroupBy(row => row.CreatedOn.Hour)
                                            .Select(grp => new
                                            {
                                                Hour = grp.Key,
                                                Value = grp.Average(x => x.StatValue)
                                            }).ToList();
                                List<Segment> segments = new List<Segment>();
                              // DateTime moment = DateTime.Now.Hour;
                               // int onhour = moment.Hour;

                                for (int hour = 1; hour <= 24; hour++)
                                {
                                    // To do
                                    // string hourString =hour<12?hour.ToString()+ " A.M " 
                                    var item = result.Where(x => x.Hour == hour).FirstOrDefault();
                                    if (item != null)
                                    {
                                        DateTime time = DateTime.Today.AddDays(-1).AddHours(hour);
                                        string displayTime = time.ToString("hh:mm tt");
                                        segments.Add(new Segment { Label = displayTime.ToString(), Value = item.Value });
                                    }
                                    else
                                    {

                                       // TimeSpan timespan = new TimeSpan(hour);
                                        DateTime time = DateTime.Today.AddHours(hour);
                                        string displayTime = time.ToString("hh:mm tt");
                                        segments.Add(new Segment { Label = displayTime.ToString(), Value = 0 });
                                        
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

                    Expression<Func<SummaryStatistics, bool>> expression = (p => p.StatName == statName && p.DeviceId == MongoDB.Bson.ObjectId.Parse(deviceId));
                    var statsHourly = summaryRepository.Find(expression);
                    var result = statsHourly
                                .GroupBy(row => row.CreatedOn.Date)
                                .Select(grp => new
                                {
                                   Date  = grp.Key,
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


       


    }
}
