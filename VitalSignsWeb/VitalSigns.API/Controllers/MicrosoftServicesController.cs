using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using MongoDB.Bson.Serialization;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Globalization;
using System.Dynamic;
using Microsoft.AspNet.Authorization;

namespace VitalSigns.API.Controllers
{
    [Authorize("Bearer")]
    [Route("services")]
    public class MicrosoftServicesController : BaseController
    {
        private IRepository<Status> statusRepository;
        private IRepository<Server> serverRepository;
        private IRepository<DailyStatistics> dailyRepository;
        private IRepository<Office365MSOLUsers> msolUsersRepository;
        private IRepository<Mailbox> mailboxRepository;
        private string DateFormat = "yyyy-MM-ddTHH:mm:ss.fffK";

        [HttpGet("user_pwd_expires")]
        public APIResponse GetUserPwdExpires(string deviceId, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Office365MSOLUsers> filterDef;
            msolUsersRepository = new Repository<Office365MSOLUsers>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = msolUsersRepository.Filter.Ne(x => x.PasswordNeverExpires, null);
                }
                else
                {
                    filterDef = msolUsersRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        msolUsersRepository.Filter.Ne(x => x.PasswordNeverExpires, null);
                }
                if (!isChart)
                {
                    var resultlist = msolUsersRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            PrincipalName = x.UserPrincipalName,
                            PwdNeverExpires = x.PasswordNeverExpires.Value,
                            StrongPwdRequired = x.StrongPasswordRequired.Value
                        }).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(resultlist);
                }
                else
                {
                    result = msolUsersRepository.Collection.Aggregate()
                        .Match(filterDef)
                        .Group(x => new { x.PasswordNeverExpires }, g => new { label = g.Key, value = g.Count() })
                        .ToList()
                        .Select(x => new Segment
                        {
                            Label = x.label.PasswordNeverExpires.ToString(),
                            Value = x.value
                        }).ToList();
                    List<Serie> series = new List<Serie>();
                    var segments = new List<Segment>();

                    foreach (var item in result)
                    {
                        if (Convert.ToBoolean(item.Label) == true)
                        {
                            segments.Add(new Segment()
                            {
                                Label = "Never expires",
                                Value = item.Value
                            });
                        }
                        else
                        {
                            segments.Add(new Segment()
                            {
                                Label = "Expires",
                                Value = item.Value
                            });
                        }
                    }

                    Serie serie = new Serie();
                    serie.Title = "Count";
                    serie.Segments = segments;
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = "Password Expires";
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

        [HttpGet("strong_pwd")]
        public APIResponse GetUserStrongPwd(string deviceId, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Office365MSOLUsers> filterDef;
            msolUsersRepository = new Repository<Office365MSOLUsers>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = msolUsersRepository.Filter.Ne(x => x.StrongPasswordRequired, null);
                }
                else
                {
                    filterDef = msolUsersRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        msolUsersRepository.Filter.Ne(x => x.StrongPasswordRequired, null);
                }
                
                if (!isChart)
                {
                    var resultlist = msolUsersRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            PrincipalName = x.UserPrincipalName,
                            PwdNeverExpires = x.PasswordNeverExpires.Value,
                            StrongPwdRequired = x.StrongPasswordRequired.Value
                        }).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(resultlist);
                }
                else
                {
                    result = msolUsersRepository.Collection.Aggregate()
                        .Match(filterDef)
                        .Group(x => new { x.StrongPasswordRequired }, g => new { label = g.Key, value = g.Count() })
                        .ToList()
                        .Select(x => new Segment
                        {
                            Label = x.label.StrongPasswordRequired.ToString(),
                            Value = x.value
                        }).ToList();
                    List<Serie> series = new List<Serie>();
                    var segments = new List<Segment>();

                    foreach (var item in result)
                    {
                        if (Convert.ToBoolean(item.Label) == true)
                        {
                            segments.Add(new Segment()
                            {
                                Label = "Not required",
                                Value = item.Value
                            });
                        }
                        else
                        {
                            segments.Add(new Segment()
                            {
                                Label = "Required",
                                Value = item.Value
                            });
                        }
                    }

                    Serie serie = new Serie();
                    serie.Title = "Count";
                    serie.Segments = segments;
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = "Strong Password Required";
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

        [HttpGet("mailbox_types")]
        public APIResponse GetMailboxTypes(string deviceId, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Mailbox> filterDef;
            mailboxRepository = new Repository<Mailbox>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = mailboxRepository.Filter.Ne(x => x.MailboxType, null);
                }
                else
                {
                    filterDef = mailboxRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        mailboxRepository.Filter.Ne(x => x.MailboxType, null);
                }
                if (!isChart)
                {
                    var resultlist = mailboxRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName
                        }).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(resultlist);
                }
                else
                {
                    result = mailboxRepository.Collection.Aggregate()
                        .Match(filterDef)
                        .Group(x => new { x.MailboxType }, g => new { label = g.Key, value = g.Count() })
                        .ToList()
                        .Select(x => new Segment
                        {
                            Label = x.label.MailboxType,
                            Value = x.value
                        }).ToList();
                    List<Serie> series = new List<Serie>();
                    var segments = new List<Segment>();

                    foreach (var item in result)
                    {
                        segments.Add(new Segment()
                        {
                            Label = item.Label,
                            Value = item.Value
                        });
                    }

                    Serie serie = new Serie();
                    serie.Title = "Count";
                    serie.Segments = segments;
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = "Mailbox types";
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
    }
}

