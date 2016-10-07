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
       private IRepository<TravelerStatusSummary> travelerStatsRepository;
 


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
        public APIResponse GetAllMobileUserDevices()
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
            Response = Common.CreateResponse(result.ToList().OrderBy(x => x.UserName));
            return Response;
        }

        ///<Author>Sowmya Pathuri</Author>
        /// <summary>
        /// Returns the mobile user devices count by type
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_type")]
        public APIResponse CountUserDevicesByType()
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
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Swathi Dongari</Author>
        /// <summary>
        /// Returns all mobile user devices counts per User
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/count_per_user")]
        public APIResponse GroupByDevicesCountForUser()
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
                segment.Label = string.Format("Users with {0} device(s)", value);
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
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Durga</Author>
        /// <summary>
        /// Returns Mobile User Devices Sync Interval 
        /// </summary>
        /// <returns>Chart </returns>
        [HttpGet("mobile_user_devices/group_by_sync_interval")]
        public APIResponse GroupBySyncInterval()
        {
            mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);

            var result = mobileDevicesRepository.Collection.AsQueryable()
                                              .Select(x => new
                                              {
                                                  LastSyncTime = x.LastSyncTime


                                              }).ToList();


            List<Segment> segments = new List<Segment>();
            double deviceSyncLast15Min = result.Where(x => x.LastSyncTime > (DateTime.Now.ToUniversalTime().AddMinutes(-15)) && x.LastSyncTime <= DateTime.Now.ToUniversalTime()).Count();
            double deviceSyncBetween15to30 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-15) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-30)).Count();
            double deviceSyncBetween30to60 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-30) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-60)).Count();
            double deviceSyncBetween60to120 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-60) && x.LastSyncTime >= DateTime.Now.ToUniversalTime().AddMinutes(-120)).Count();
            double deviceSyncGreater120 = result.Where(x => x.LastSyncTime < DateTime.Now.ToUniversalTime().AddMinutes(-120)).Count();
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
            Response = Common.CreateResponse(chart);
            return Response;
        }

        ///<Author>Sowjanya Korumilli</Author>
        /// <summary>
        /// Returns userdevices count by os
        /// </summary>
        /// <returns>Chart</returns>
        [HttpGet("mobile_user_devices/count_by_os")]
        public APIResponse CountUserDevicesByOS()
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
            Response = Common.CreateResponse(chart);
            return Response;
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
                    LastScan =Convert.ToString( x.LastUpdate.Value),
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
                DateUpdated = Convert.ToString(x.DateUpdated)


            });
            return result.ToList();
        }

        [HttpGet("database")]

        public APIResponse GetDatabase(string filter_by, string filter_value, string order_by, string order_type, string group_by, string top_x, bool get_chart)
        {
            List<ServerDatabase> data = null;
            List<Segment> segments = new List<Segment>();
            List<BsonDocument> bsonDocs;

            try
            {
                var builder = Builders<Database>.Filter;
                databaseRepository = new Repository<Database>(ConnectionString);
                // filter_by is not specified - getting all documents in the collection
                if (string.IsNullOrEmpty(filter_by))
                {
                    // group_by is not specified - getting a flat list of entries, no grouping
                    if (string.IsNullOrEmpty(group_by))
                    {
                        var res = databaseRepository.Collection.Aggregate()
                                    .Match(_ => true).ToList();

                        data = res.Select(x => new ServerDatabase
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
                            ScanDateTime = Convert.ToString(x.ScanDateTime.Value),
                            ReplicaId = x.ReplicaId,
                            DocumentCount = x.DocumentCount,
                            Categories = x.Categories,
                            PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                        }).ToList();
                        // order_by is specified - sorting resulting data by th field specified in order_by in order specified by order_type (asc/desc)
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            data = order_type == "asc" ? data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        if (!string.IsNullOrEmpty(top_x))
                        {
                            data = data.Take(Convert.ToInt32(top_x)).ToList();
                        }
                    }
                    // group_by is specified - grouping data by the field specified by the group_by parameter
                    else
                    {
                        bsonDocs = databaseRepository.Collection.Aggregate()
                                    .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            bsonDocs = order_type == "asc" ? bsonDocs.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : bsonDocs.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (BsonDocument doc in bsonDocs)
                        {
                            if (!doc["_id"].IsBsonNull)
                            {
                                Segment segment = new Segment()
                                {
                                    //Might have to add additional types for support.  Format is IfThis ? DoThis : Else
                                    Label = doc["_id"].IsString ? doc["_id"].AsString :
                                    (doc["_id"].IsInt32 ? Convert.ToString(doc["_id"].AsInt32) :
                                    (doc["_id"].IsBoolean ? Convert.ToString(doc["_id"].AsBoolean) : Convert.ToString(doc["_id"].AsBsonValue))),
                                    Value = doc["count"].AsInt32
                                };
                                segments.Add(segment);
                            }
                        }
                    }
                }
                // filter_by is specified - getting a subset of documents using the value specified in the filter_value parameter
                else
                {
                    // group_by is not specified - getting a flat list of entries, no grouping
                    if (string.IsNullOrEmpty(group_by))
                    {
                        bool flag;
                        if (Boolean.TryParse(filter_value, out flag))
                        {
                            data = databaseRepository.Find(builder.Eq(filter_by, flag))
                                    .Select(x => new ServerDatabase
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
                                        ScanDateTime = Convert.ToString(x.ScanDateTime.Value),
                                        ReplicaId = x.ReplicaId,
                                        DocumentCount = x.DocumentCount,
                                        Categories = x.Categories,
                                        PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                    }).ToList();
                        }
                        else
                        {
                            data = databaseRepository.Find(builder.Eq(filter_by, filter_value))
                                    .Select(x => new ServerDatabase
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
                                        ScanDateTime = Convert.ToString(x.ScanDateTime.Value),
                                        ReplicaId = x.ReplicaId,
                                        DocumentCount = x.DocumentCount,
                                        Categories = x.Categories,
                                        PercentQuota = Convert.ToDouble(x.Quota > 0 ? Math.Round(Convert.ToDouble(Convert.ToDouble(x.FileSize) / Convert.ToDouble(x.Quota) * 100), 1) : 0.0)
                                    }).ToList();
                        }
                        // order_by is specified - sorting resulting data by th field specified in order_by in order specified by order_type (asc/desc)
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            data = order_type == "asc" ? data.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : data.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        if (!string.IsNullOrEmpty(top_x))
                        {
                            data = data.Take(Convert.ToInt32(top_x)).ToList();
                        }
                    }
                    // group_by is specified - grouping data by the field specified by the group_by parameter
                    else
                    {
                        bool flag;
                        if (Boolean.TryParse(filter_value, out flag))
                        {
                            bsonDocs = databaseRepository.Collection.Aggregate()
                               .Match(builder.Eq(filter_by, flag))
                               .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        }
                        else
                        {
                            bsonDocs = databaseRepository.Collection.Aggregate()
                               .Match(builder.Eq(filter_by, filter_value))
                               .Group(new BsonDocument { { "_id", "$" + group_by }, { "count", new BsonDocument("$sum", 1) } }).ToList();
                        }
                        if (!string.IsNullOrEmpty(order_by) && !string.IsNullOrEmpty(order_type))
                        {
                            var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                            bsonDocs = order_type == "asc" ? bsonDocs.OrderBy(x => propertyInfo.GetValue(x, null)).ToList() : bsonDocs.OrderByDescending(x => propertyInfo.GetValue(x, null)).ToList();
                        }
                        foreach (BsonDocument doc in bsonDocs)
                        {
                            if (!doc["_id"].IsBsonNull)
                            {
                                if (!doc["_id"].IsBsonNull)
                                {
                                    var labelVal = "";
                                    labelVal = doc["_id"].IsString ? doc["_id"].AsString :
                                        (doc["_id"].IsInt32 ? Convert.ToString(doc["_id"].AsInt32) :
                                        (doc["_id"].IsBoolean ? Convert.ToString(doc["_id"].AsBoolean) : Convert.ToString(doc["_id"].AsBsonValue)));
                                    if (doc["_id"].AsString == "")
                                    {
                                        labelVal = "Not Assigned";
                                    }
                                    Segment segment = new Segment()
                                    {
                                        Label = labelVal,
                                        Value = doc["count"].AsInt32
                                    };
                                    segments.Add(segment);
                                }
                            }
                        }
                    }
                }
                    
                // If a call was made without the group_by parameter, return data set as is,
                // otherwise, return data as a chart
                if (!string.IsNullOrEmpty(get_chart.ToString()))
                {
                    if (!get_chart)
                    {
                        Response = Common.CreateResponse(data);
                    }
                    else
                    {
                        if (segments.Count == 0)
                        {
                            foreach (ServerDatabase doc in data)
                            {
                                var bsondoc = doc.ToBsonDocument();
                                var propertyInfo = typeof(ServerDatabase).GetProperty(order_by);
                                Segment segment = new Segment()
                                {
                                    Label = doc.Title + " (" + doc.DeviceName + ")",
                                    Value = Convert.ToDouble(propertyInfo.GetValue(doc))
                                };
                                segments.Add(segment);
                            }
                        }
                        List<Serie> dbseries = new List<Serie>();
                        Serie dbserie = new Serie();
                        segments.RemoveAll(item => item.Label == null);
                        segments.RemoveAll(item => item.Label == "");
                        dbserie.Title = "Data";
                        dbserie.Segments = segments;
                        dbseries.Add(dbserie);
                        Chart chart = new Chart();
                        chart.Title = "Database";
                        chart.Series = dbseries;
                        Response = Common.CreateResponse(chart);
                    }
                }
                else
                {
                    Response = Common.CreateResponse(data);
                }
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
                    DateTimeDown =Convert.ToString( x.DateTimeDown),
                    DateTimeUp = Convert.ToString(x.DateTimeUp),
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

        public APIResponse GetMonitoredTasks(string deviceid, bool is_monitored)
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
                        SecondaryStatus = monitored.SecondaryStatus,
                        StatusSummary = monitored.StatusSummary,
                        LastUpdated =Convert.ToString( monitored.LastUpdated)

                    });
                    
                }

                var monitoredData = monitoredTasks.Where(x => x.Monitored == is_monitored);

                Response = Common.CreateResponse(monitoredData);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        [HttpGet("{deviceid}/traveler_mailstats")]

        public APIResponse Travelerstats(string deviceid)
        {
            travelerStatsRepository = new Repository<TravelerStatusSummary>(ConnectionString);
            try
            {


                Expression<Func<TravelerStatusSummary, bool>> expression = (p => p.DeviceId == deviceid);
                var result = travelerStatsRepository.Find(expression).Select(x => new TravelerHealth
                {
                    DeviceId = x.DeviceId,
                    MailServerName = x.MailServerName,
                    c_000_001 = x.c_000_001,
                    c_001_002 = x.c_001_002,
                    c_002_005 = x.c_002_005,
                    c_005_010 = x.c_005_010,
                    c_010_030 = x.c_010_030,
                    c_030_060 = x.c_030_060,
                    c_060_120 = x.c_060_120,
                    c_120_INF = x.c_120_INF,
                    DateUpdated = Convert.ToString(x.DateUpdated)


                }).ToList();

                Response = Common.CreateResponse(result);

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }

            return Response;


        }

        [HttpGet("{device_id}/clusterhealth")]
        public APIResponse GetClusterHealth(string device_id)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            try
            {
                var clusterData = statusRepository.Find(x => x.DeviceId == device_id).FirstOrDefault();
                if (clusterData != null)
                {
                    string clusterName = clusterData.ClusterName;
                    Expression<Func<Status, bool>> expression = (p => p.ClusterName == clusterName);
                    var result = statusRepository.Find(expression).Select(x => new StatusBox
                    {
                        DeviceId = x.DeviceId,
                        DeviceName = x.DeviceName,
                        ClusterName = x.ClusterName,
                        ClusterSecondsOnQueue = x.ClusterSecondsOnQueue,
                        ClusterSecondsOnQueueAverage = x.ClusterSecondsOnQueueAverage,
                        ClusterWorkQueueDepth = x.ClusterWorkQueueDepth,
                        ClusterWorkQueueDepthAverage = x.ClusterWorkQueueDepthAverage,
                        ClusterAvailability = x.ClusterAvailability,
                        ClusterAvailabilityThreshold = x.ClusterAvailabilityThreshold,
                        LastUpdated = Convert.ToString(x.LastUpdated.Value),
                        ClusterAnalysis = x.ClusterAnalysis

                    }).ToList();
                    Response = Common.CreateResponse(result);
                }
                else
                {
                    Response = Common.CreateResponse(null, "Data Error", "Cluster data not found");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }
    }
}

        