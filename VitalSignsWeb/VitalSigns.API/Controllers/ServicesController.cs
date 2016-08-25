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
        private IRepository<DiskHealth> diskHealthRepository;

        private IRepository<Server> serverRepository;

        [HttpGet("status_summary")]
        public IEnumerable<Segment> ServersStatusSummary()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.Collection.Aggregate()
                                               .Group(x => x.StatusCode, g => new { label = g.Key, value = g.Count() })
                                               .Project(x => new Segment
                                               {
                                                   Label = x.label,
                                                   Value = x.value
                                               });
            return result.ToEnumerable();
        }

        ///<Author>Sowmya Pathuri</Author>
        /// <summary>
        ///    Summary of all mail routing 
        /// </summary>
        /// <returns></returns>
        [HttpGet("mail_status")]
        public IEnumerable<Segment> GetAllMailRoutings()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            Expression<Func<Status, bool>> expression = (p => p.Location != "" && p.Type == "Domino");
            var result = statusRepository.Find(expression).Select(x => new
            {
                PendingMail = x.PendingMail,
                DeadMail = x.DeadMail,
                HeldMail = 0,
                //UserCount = x.UserCount,
                //ResponseTime = x.ResponseTime,
                DownMinutes = 0

            });
            List<Segment> segments = new List<Segment>();

            int? pendingMail = result.Sum(x => x.PendingMail);
            segments.Add(new Segment { Label = "Pending Mail", Value = pendingMail.Value });

            int? deadMail = result.Sum(x => x.DeadMail);
            segments.Add(new Segment { Label = "Dead Mail", Value = deadMail.Value });

            return segments;
        }

        ///<Author>Sowmya Pathuri</Author>
        /// <summary>
        ///    Get status box info for a given server or service 
        /// </summary>
        /// <returns></returns>
        [HttpGet("status_widget/{id}")]
        public StatusBox GetStatusById(string id)
        {
            statusRepository = new Repository<Status>(ConnectionString);
            Expression<Func<Status, bool>> expression = (p => p.Id == id);
            var result = statusRepository.Find(expression).Select(x => new StatusBox
            {
                Type = x.Type,
                Name = x.Name,
                LastScan = x.LastUpdated,
                Status = x.StatusCode,
                Icon = ""
            });
            return result.FirstOrDefault();
        }

        ///<Author>Swathi Dongari</Author>
        /// <summary>
        ///   
        /// </summary>
        /// <returns></returns>
        [HttpGet("status_summary_by_type")]
        public IEnumerable<StatusSummary> GetStatusSummaryByType()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.Collection.Aggregate()
                                        .Project(x => new StatusBox
                                        {
                                            Type = x.Type,
                                            Status = x.StatusCode
                                        }).ToList();
            List<string> typeList = result.Select(x => x.Type).Distinct().ToList();
            List<StatusSummary> summaryList = new List<StatusSummary>();
            foreach (string type in typeList)
            {
                summaryList.Add(new StatusSummary
                {
                    Type = type,
                    Ok = result.Where(x => x.Type == type && x.Status == "OK").Count(),
                    NotResponding = result.Where(x => x.Type == type && x.Status == "Not Responding").Count(),
                    Issue = result.Where(x => x.Type == type && x.Status == "Issue").Count(),
                    Maintenance = result.Where(x => x.Type == type && x.Status == "Maintenance").Count()
                });
            }
            return summaryList;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet("services")]
        public IEnumerable<ServiceStatus> GetAllServerServices()
        {
            statusRepository = new Repository<Status>(ConnectionString);
            var result = statusRepository.All().AsQueryable()
                                 .Select(x => new ServiceStatus
                                 {
                                     Id = x.Id,
                                     Icon = x.Icon,
                                     Country = x.Location,
                                     Name = x.Name,
                                     Version = x.Version,
                                     Description = x.Description,
                                     Status = x.StatusCode

                                 });
            return result.ToList();
        }
        [HttpGet("diskhealth")]

        public IEnumerable<ServerDiskStatus> GetStatusOfAllServerDiskDrives()
        {
            diskHealthRepository = new Repository<DiskHealth>(ConnectionString);
            var result = diskHealthRepository.All();
            List<ServerDiskStatus> serverDiskStatusList = new List<ServerDiskStatus>();
            foreach (DiskHealth diskResult in result)
            {
                ServerDiskStatus serverDiskStatus = new ServerDiskStatus();
                serverDiskStatus.Id = diskResult.Id;
                serverDiskStatus.Name = diskResult.ServerName;
                foreach (Drive drive in diskResult.Drives)
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


                serverDiskStatusList.Add(serverDiskStatus);

            }
            return serverDiskStatusList;
        }
        [HttpGet("windows_services/{id}")]
        public IEnumerable<WindowsService> GetWindowsService(string id)
        {
            serverRepository = new Repository<Server>(ConnectionString);

            Expression<Func<Server, bool>> expression = (p => p.Id == id);
            Server server = serverRepository.Find(expression).FirstOrDefault();
            if (server.WindowServices != null)
            {
                var result = server.WindowServices.Select(x => new WindowsService
                {
                    DisplayName = x.DisplayName,
                    Monitored = x.Monitored,
                    ServerRequired = x.ServerRequired,
                    ServiceName = x.ServiceName,
                    StartupMode = x.StartupMode,
                    Status = x.Status
                });
                return result.ToList();
            }
            else
                return null;
        }


    }
}
