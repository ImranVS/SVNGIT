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
        public APIResponse GetDailyStat(int deviceId, string statName, string operation)
        {
            dailyRepository = new Repository<DailyStatistics>(ConnectionString);

            try
            {
                if (deviceId == 0 && !string.IsNullOrEmpty(statName))
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName);
                    var result = dailyRepository.Find(expression).Select(x => new StatsData
                    {
                        DeviceId = x.DeviceId,
                        StatName = x.StatName,
                        StatValue = x.StatValue

                    }).ToList();
                    Response = Common.CreateResponse(result);

                }
                else
                {
                    Expression<Func<DailyStatistics, bool>> expression = (p => p.StatName == statName && p.DeviceId == deviceId);

                    if (string.IsNullOrEmpty(operation) && !string.IsNullOrEmpty(statName))
                    {

                        var result = dailyRepository.Find(expression).Select(x => new StatsData
                        {
                            DeviceId = x.DeviceId,
                            StatName = x.StatName,
                            StatValue = x.StatValue

                        }).ToList();
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
                                for (int hour = 1; hour <= 24; hour++)
                                {
                                    // To do
                                    // string hourString =hour<12?hour.ToString()+ " A.M " 
                                    var item = result.Where(x => x.Hour == hour).FirstOrDefault();
                                    if (item != null)
                                    {
                                        segments.Add(new Segment { Label = hour.ToString(), Value = item.Value });
                                    }
                                    else
                                    {
                                        segments.Add(new Segment { Label = hour.ToString(), Value = 0 });
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



       

    }
}
