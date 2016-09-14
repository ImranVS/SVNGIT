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

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class DashBoardController : BaseController
    {
        private IRepository<MobileDevices> mobileDevicesRepository;
        private IRepository<DiskHealth> diskHealthRepository;
        private IRepository<StatusDetails> statusdetailsRepository;

        private IRepository<Status> statusRepository;
        private IRepository<TravelerStats> travelerRepository;

        private IRepository<Database> databaseRepository;
        private IRepository<Outages> outagesRepository;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/overall/memory-usage")]
        public Chart MemoryUsage(int id)
        {
            var dataContext = new DataContext();

            var aggregate = dataContext.DailyStatistics.Aggregate()
                    .Match(new BsonDocument {
                        { "device_id", id },
                        { "stat_name", "Mem.PercentUsed" }
                    })
                    .Group(new BsonDocument {
                    { "_id",
                            new BsonDocument {
                                { "hour", new BsonDocument("$hour", "$created_on") }
                            }
                        },
                    { "created_on", new BsonDocument("$min", "$created_on") },
                    { "value", new BsonDocument("$avg", "$stat_value") }
                    })
                    .Project<Segment>(new BsonDocument {
                        { "_id", 0 },
                        { "label",
                            new BsonDocument {
                                { "$dateToString",
                                       new BsonDocument {
                                            { "format", "%H" },
                                            { "date", "$created_on" }
                                        }
                                }
                            }
                        },
                        { "value", 1 }
                    })
                    .Sort(new BsonDocument("label", 1));

            Serie serie = new Serie();
            serie.Title = "Memory Usage";
            serie.Segments = aggregate.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Memory Usage";
            chart.Series = series;

            return chart;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}/overall/cpu-usage")]
        public Chart CPUUsage(int id)
        {
            var dataContext = new DataContext();

            var aggregate = dataContext.DailyStatistics.Aggregate()
                    .Match(new BsonDocument {
                        { "device_id", id },
                        { "stat_name", "Platform.System.PctCombinedCpuUtil" }
                    })
                    .Group(new BsonDocument {
                    { "_id",
                            new BsonDocument {
                                { "hour", new BsonDocument("$hour", "$created_on") }
                            }
                        },
                    { "created_on", new BsonDocument("$min", "$created_on") },
                    { "value", new BsonDocument("$avg", "$stat_value") }
                    })
                    .Project<Segment>(new BsonDocument {
                        { "_id", 0 },
                        { "label",
                            new BsonDocument {
                                { "$dateToString",
                                       new BsonDocument {
                                            { "format", "%H" },
                                            { "date", "$created_on" }
                                        }
                                }
                            }
                        },
                        { "value", 1 }
                    })
                    .Sort(new BsonDocument("label", 1));

            Serie serie = new Serie();
            serie.Title = "CPU Usage";
            serie.Segments = aggregate.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "CPU Usage";
            chart.Series = series;

            return chart;
        }

        ///<Author>Kiran Dadireddy</Author>
        /// <summary>
        /// Returns all mobile user devices
        /// </summary>
        /// <returns> </returns>
        [HttpGet("mobile_user_devices")]
        public IEnumerable<MobileUserDevice> GetAllMobileUserDevices()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection
                                 .AsQueryable()
                                 .Select(x => new MobileUserDevice
                                 {
                                     UserName = x.UserName,
                                     Device = x.DeviceName,
                                     Notification = x.NotificationType,
                                     OperatingSystem = x.OSType,
                                     LastSyncTime = x.LastSyncTime,
                                     Access = x.Access,
                                     DeviceId = x.DeviceID
                                 });
            return result.ToList();
        }

        ///<Author>Sowmya Pathuri</Author>
        /// <summary>
        /// Returns the mobile user devices count by type
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_type")]
        public Chart CountUserDevicesByType()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                             .Group(x => x.DeviceType, g => new { label = g.Key, value = g.Count() })
                                             .Project(x => new Segment
                                             {
                                                 Label = x.label,
                                                 Value = x.value
                                             });
            Serie serie = new Serie();
            serie.Title = "Mobile Devices";
            serie.Segments = result.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Mobile Devices";
            chart.Series = series;

            return chart;
        }

        ///<Author>Swathi Dongari</Author>
        /// <summary>
        /// Returns all mobile user devices counts per User
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/count_per_user")]
        public Chart GroupByDevicesCountForUser()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                                .Group(x => x.UserName, g => new { label = g.Key, value = g.Count() })
                                                .Project(x => new Segment
                                                {
                                                    Label = x.label,
                                                    Value = x.value
                                                }).ToList();
            List<double> deviceCount = result.Select(x => x.Value).Distinct().ToList();
            List<Segment> segments = new List<Segment>();
            foreach (double value in deviceCount)
            {
                Segment segment = new Segment();
                segment.Label = string.Format("Users with {0} device", value);
                segment.Value = result.Where(x => x.Value == value).Count();
                segments.Add(segment);

            }

            Serie serie = new Serie();
            serie.Title = "Device count / user";
            serie.Segments = segments;
            List<Serie> series = new List<Serie>();
            series.Add(serie);
            Chart chart = new Chart();
            chart.Title = "Device count / user";
            chart.Series = series;

            return chart;
        }

        ///<Author>Durga</Author>
        /// <summary>
        /// Returns Mobile User Devices Sync Interval 
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/group_by_sync_interval")]
        public Chart GroupBySyncInterval()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);

            var result = mobileDevicesRepository.Collection.AsQueryable()
                                              .Select(x => new
                                              {
                                                  LastSyncTime = x.LastSyncTime


                                              }).ToList();


            List<Segment> segments = new List<Segment>();
            double deviceSyncLast15Min = result.Where(x => x.LastSyncTime <= (DateTime.Now.AddMinutes(-15)) && x.LastSyncTime > DateTime.Now).Count();
            double deviceSyncBetween15to30 = result.Where(x => x.LastSyncTime > DateTime.Now.AddMinutes(-15) && x.LastSyncTime <= DateTime.Now.AddMinutes(-30)).Count();
            double deviceSyncBetween30to60 = result.Where(x => x.LastSyncTime > DateTime.Now.AddMinutes(-30) && x.LastSyncTime <= DateTime.Now.AddMinutes(-60)).Count();
            double deviceSyncBetween60to120 = result.Where(x => x.LastSyncTime > DateTime.Now.AddMinutes(-60) && x.LastSyncTime <= DateTime.Now.AddMinutes(-120)).Count();
            double deviceSyncGreater120 = result.Where(x => x.LastSyncTime > DateTime.Now.AddMinutes(-120)).Count();
            if (deviceSyncLast15Min > 0)
            {
                segments.Add(new Segment { Label = "Within 15 mins.", Value = deviceSyncLast15Min });
            }

            if (deviceSyncBetween15to30 > 0)
            {
                segments.Add(new Segment { Label = "Between 15-30 mins.", Value = deviceSyncBetween15to30 });
            }
            if (deviceSyncBetween30to60 > 0)
            {
                segments.Add(new Segment { Label = "Between 30-60 mins.", Value = deviceSyncBetween30to60 });
            }
            if (deviceSyncBetween60to120 > 0)
            {
                segments.Add(new Segment { Label = "Between 60-120 mins.", Value = deviceSyncBetween60to120 });
            }

            if (deviceSyncGreater120 > 0)
            {
                segments.Add(new Segment { Label = "Greater than 120 mins.", Value = deviceSyncGreater120 });
            }

            Serie serie = new Serie();
            serie.Title = "Sync times";
            serie.Segments = segments.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Sync times";
            chart.Series = series;

            return chart;
        }

        ///<Author>Sowjanya Korumilli</Author>
        /// <summary>
        /// Returns userdevices count by os
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_os")]
        public Chart CountUserDevicesByOS()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
            var result = mobileDevicesRepository.Collection.Aggregate()
                                           .Group(x => x.OSType, g => new { label = g.Key, value = g.Count() })
                                           .Project(x => new Segment
                                           {
                                               Label = x.label,
                                               Value = x.value
                                           });
            Serie serie = new Serie();
            serie.Title = "Mobile devices OS for all Servers";
            serie.Segments = result.ToList();

            List<Serie> series = new List<Serie>();
            series.Add(serie);

            Chart chart = new Chart();
            chart.Title = "Mobile devices OS for all Servers";
            chart.Series = series;

            return chart;
        }

        [HttpGet("{id}/overall/disk-space-v2")]

        public ServerDiskStatus GetStatusOfServerDiskDrives(string id)
        {
            diskHealthRepository = new Repository<DiskHealth>(ConnectionString);

            Expression<Func<DiskHealth, bool>> expression = (p => p.Id == id);
            var result = diskHealthRepository.Find(expression).FirstOrDefault();


            ServerDiskStatus serverDiskStatus = new ServerDiskStatus();
            serverDiskStatus.Id = result.Id;
            foreach (Drive drive in result.Drives)
            {
                serverDiskStatus.Drives.Add(new DiskDriveStatus
                {
                    DiskFree = drive.DiskFree,
                    DiskSize = drive.DiskSize,
                    DiskName = drive.DiskName,
                    DiskUsed = drive.DiskSize - drive.DiskFree,
                    Status = drive.Status,
                    PercentFree = drive.PercentFree,
                    Threshold = drive.Threshold,
                    LastUpdated = drive.LastUpdated
                });
            }

            return serverDiskStatus;
        }

        [HttpGet("{device_id}/health-assessment")]
        public APIResponse GetHealthAssessment(string device_id)
        {
            statusdetailsRepository = new Repository<StatusDetails>(ConnectionString);
            try
            {

                Expression<Func<StatusDetails, bool>> expression = (p => p.DeviceId == device_id);
                var result = statusdetailsRepository.Find(expression).Select(x => new HealthAssessment
                {
                    DeviceId = x.DeviceId,
                    Type = x.Type,
                    Category = x.category,
                    LastScan = x.LastUpdate,
                    TestName = x.TestName,
                    Result = x.Result,
                    Details = x.Details
                });
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("{deviceid}/traveler-health")]
        public APIResponse GetTravelerHealth(string deviceid)
        {
            try
            {
                statusRepository = new Repository<Status>(ConnectionString);

                Expression<Func<Status, bool>> expression = (p => p.DeviceId == deviceid);
                var result = statusRepository.Find(expression).Select(x => new TravelerHealth
                {
                    DeviceId= x.DeviceId,
                    ResourceConstraint = x.ResourceConstraint,
                    TravelerDetails = x.TravelerDetails,
                    TravelerHeartBeat = x.TravelerHeartBeat,
                    TravelerServlet = x.TravelerServlet,
                    TravelerUsers = x.TravelerUsers,
                    TravelerHA = x.TravelerHA,
                    TravelerIncrementalSyncs = x.TravelerIncrementalSyncs,
                    HttpStatus = x.HttpStatus,
                    TravelerDevicesAPIStatus = x.TravelerDevicesAPIStatus,
                });
                Response = Common.CreateResponse(result);
                return Response;
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

        [HttpGet("{id}/traveler-mailservers")]
        public IEnumerable<TravelerHealth> GetTravelerHealthGrid(string id)
        {
            travelerRepository = new Repository<TravelerStats>(ConnectionString);

            Expression<Func<TravelerStats, bool>> expression = (p => p.Id == id);
            var result = travelerRepository.Find(expression).Select(x => new TravelerHealth
            {
                // ID = x.ID,
                MailServerName = x.MailServerName,
                DateUpdated = x.DateUpdated


            });
            return result.ToList();
        }


        [HttpGet("{device_id}/database")]
        public APIResponse GetDatabase(string device_id)
        {

            databaseRepository = new Repository<Database>(ConnectionString);

            try
            {
                Expression<Func<Database, bool>> expression = (p => p.DeviceId == device_id);
                var result = databaseRepository.Find(expression).Select(x => new ServerDatabase
                {
                    DeviceId = x.DeviceId,
                    Title = x.Title,
                    
                    DeviceName = x.DeviceName,

                    Status = x.Status,
                    
                    Folder = x.Folder,                

                    FolderCount = x.FolderCount,
                    Details = x.Details,
                    FileName = x.FileName,
                    DesignTemplateName = x.DesignTemplateName,
                    FileSize = x.FileSize,
                    Quota = x.Quota,
                    InboxDocCount = x.InboxDocCount,
                    ScanDateTime = x.ScanDateTime,
                    ReplicaId = x.ReplicaId,
                    DocumentCount = x.DocumentCount,
                    Categories = x.Categories
                }).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpGet("{device_id}/outages")]
        public APIResponse GetOutages(string device_id)
        {
            outagesRepository = new Repository<Outages>(ConnectionString);
            try
            {
                Expression<Func<Outages, bool>> expression = (p => p.DeviceId == device_id);
                var result = outagesRepository.Find(expression).Select(x => new Outage
                {
                    DeviceId = x.DeviceId,
                    DeviceName = x.DeviceName,
                    DateTimeDown = x.DateTimeDown,
                    DateTimeUp = x.DateTimeUp,
                }).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        [HttpGet("{deviceid}/monitoredtasks")]

        public APIResponse GetMonitoredTasks(string deviceid)
        {

            statusRepository = new Repository<Status>(ConnectionString);
            try
            {



                Expression<Func<Status, bool>> expression = (p => p.DeviceId == deviceid);
                var result = statusRepository.Find(expression).FirstOrDefault();


               List< MonitoredTasks> monitoredTasks = new List<MonitoredTasks>();
               // dominoservertaskStatus.Id = result.DeviceId;
                foreach (DominoServerTask monitored in result.DominoServerTasks)
                {
                    monitoredTasks.Add(new MonitoredTasks
                    {
                        // DeviceId = monitored.
                        TaskName = monitored.TaskName,
                        Monitored = monitored.Monitored,
                        PrimaryStatus = monitored.PrimaryStatus,
                        SecondaryStatus = monitored.SecondaryStatus

                    });
                    
                }
                Response = Common.CreateResponse(monitoredTasks);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
    }
}

        