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

        [HttpGet("top_mailboxes")]
        public APIResponse GetTopMailboxes(string deviceId, int topx = 5, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Mailbox> filterDef;
            mailboxRepository = new Repository<Mailbox>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = mailboxRepository.Filter.Ne(x => x.TotalItemSizeMb, null) &
                        mailboxRepository.Filter.Ne(x => x.MailboxType, "DiscoveryMailbox");
                }
                else
                {
                    filterDef = mailboxRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        mailboxRepository.Filter.Ne(x => x.MailboxType, "DiscoveryMailbox") &
                        mailboxRepository.Filter.Ne(x => x.TotalItemSizeMb, null);
                }
                if (!isChart)
                {
                    var resultlist = mailboxRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            TotalItemSizeMB = x.TotalItemSizeMb
                        }).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(resultlist);
                }
                else
                {
                    var resultlist = mailboxRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            TotalItemSizeMB = x.TotalItemSizeMb
                        }).OrderByDescending(x => x.TotalItemSizeMB).Take(topx).ToList();
                    
                    List<Segment> segments = new List<Segment>();
                    Segment segment = new Segment();
                    foreach (var doc in resultlist)
                    {
                        segment = new Segment()
                        {
                            Label = doc.Name,
                            Value = Math.Round(Convert.ToDouble(doc.TotalItemSizeMB)/1024,1)
                        };
                        segments.Add(segment);
                    }
                    List<Serie> series = new List<Serie>();
                    Serie serie = new Serie();
                    serie.Title = "Size";
                    serie.Segments = segments;
                    series.Add(serie);
                    Chart chart = new Chart();
                    chart.Title = "";
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

        [HttpGet("last_logon")]
        public APIResponse GetLastLogon(string deviceId, bool isChart = true)
        {
            DateTime dtStart1;
            DateTime dtStart2;
            DateTime dtStart7;
            DateTime dtEnd;
            string startDate1 = "";
            string startDate2 = "";
            string startDate7 = "";
            string endDate = "";
            string label1 = "1 day ago";
            string label2 = "2 days ago";
            string label7 = "7 days ago";
            string labelmore = "more than 7 days ago";
            int value1 = 0;
            int value2 = 0;
            int value7 = 0;
            int valuemore = 0;
            List<Segment> result = new List<Segment>();
            FilterDefinition<Mailbox> filterDef;
            
            try
            {
                startDate1 = DateTime.Now.AddDays(-1).ToString(DateFormat);
                startDate2 = DateTime.Now.AddDays(-2).ToString(DateFormat);
                startDate7 = DateTime.Now.AddDays(-7).ToString(DateFormat);
                endDate = DateTime.Now.ToString(DateFormat);

                dtStart1 = DateTime.ParseExact(startDate1, DateFormat, CultureInfo.InvariantCulture);
                dtStart2 = DateTime.ParseExact(startDate2, DateFormat, CultureInfo.InvariantCulture);
                dtStart7 = DateTime.ParseExact(startDate7, DateFormat, CultureInfo.InvariantCulture);
                dtEnd = DateTime.ParseExact(endDate, DateFormat, CultureInfo.InvariantCulture).AddDays(1);

                dtStart1 = DateTime.SpecifyKind(dtStart1, DateTimeKind.Utc);
                dtStart2 = DateTime.SpecifyKind(dtStart2, DateTimeKind.Utc);
                dtStart7 = DateTime.SpecifyKind(dtStart7, DateTimeKind.Utc);
                dtEnd = DateTime.SpecifyKind(dtEnd, DateTimeKind.Utc);

                mailboxRepository = new Repository<Mailbox>(ConnectionString);
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = mailboxRepository.Filter.Ne(x => x.LastLogonTime, null);
                }
                else
                {
                    filterDef = mailboxRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        mailboxRepository.Filter.Ne(x => x.LastLogonTime, null);
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
                    result = mailboxRepository.Find(filterDef)
                        .Select(x => new Segment
                        {
                            Label = x.LastLogonTime.Value.ToString(),
                            Value = 1
                        }).ToList();
                    List<Serie> series = new List<Serie>();
                    var segments = new List<Segment>();
                    foreach (var item in result)
                    {
                        if (Convert.ToDateTime(item.Label) >= dtStart1 && Convert.ToDateTime(item.Label) <= dtEnd)
                        {
                            value1 += 1;
                        }
                        else if (Convert.ToDateTime(item.Label) >= dtStart2 && Convert.ToDateTime(item.Label) <= dtStart1)
                        {
                            value2 += 1;
                        }
                        else if (Convert.ToDateTime(item.Label) >= dtStart7 && Convert.ToDateTime(item.Label) <= dtStart2)
                        {
                            value7 += 1;
                        }
                        else if (Convert.ToDateTime(item.Label) < dtStart7)
                        {
                            valuemore += 1;
                        }           
                    }
                    segments.Add(new Segment()
                    {
                        Label = label1,
                        Value = value1
                    });
                    segments.Add(new Segment()
                    {
                        Label = label2,
                        Value = value2
                    });
                    segments.Add(new Segment()
                    {
                        Label = label7,
                        Value = value7
                    });
                    segments.Add(new Segment()
                    {
                        Label = labelmore,
                        Value = valuemore
                    });
                    Serie serie = new Serie();
                    serie.Title = "Count";
                    serie.Segments = segments;
                    series.Add(serie);

                    Chart chart = new Chart();
                    chart.Title = "Last logon";
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

        [HttpGet("top_inactive_mailboxes")]
        public APIResponse GetTopInactiveMailboxes(string deviceId, int topx = 5, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Mailbox> filterDef;
            mailboxRepository = new Repository<Mailbox>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = mailboxRepository.Filter.Ne(x => x.InactiveDaysCount, null);
                }
                else
                {
                    filterDef = mailboxRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        mailboxRepository.Filter.Ne(x => x.InactiveDaysCount, null);
                }
                if (!isChart)
                {
                    var resultlist = mailboxRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            TotalItemSizeMB = x.TotalItemSizeMb
                        }).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(resultlist);
                }
                else
                {
                    var resultlist = mailboxRepository.Find(filterDef)
                        .Select(x => new Office365Users
                        {
                            Name = x.DisplayName,
                            InactiveDaysCount = x.InactiveDaysCount
                        }).OrderByDescending(x => x.InactiveDaysCount).Take(topx).ToList();

                    List<Segment> segments = new List<Segment>();
                    Segment segment = new Segment();
                    foreach (var doc in resultlist)
                    {
                        segment = new Segment()
                        {
                            Label = doc.Name,
                            Value = doc.InactiveDaysCount
                        };
                        segments.Add(segment);
                    }
                    List<Serie> series = new List<Serie>();
                    Serie serie = new Serie();
                    serie.Title = "Count";
                    serie.Segments = segments;
                    series.Add(serie);
                    Chart chart = new Chart();
                    chart.Title = "";
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

        [HttpGet("active_inactive_users")]
        public APIResponse GetActiveInactiveUsers(string deviceId, bool isChart = true)
        {
            List<Segment> result = new List<Segment>();
            FilterDefinition<Mailbox> filterDef;
            mailboxRepository = new Repository<Mailbox>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(deviceId))
                {
                    filterDef = mailboxRepository.Filter.Ne(x => x.InactiveDaysCount, null);
                }
                else
                {
                    filterDef = mailboxRepository.Filter.Eq(x => x.DeviceId, deviceId) &
                        mailboxRepository.Filter.Ne(x => x.InactiveDaysCount, null);
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
                        .Project(x => new { inactive = x.InactiveDaysCount > 7 ? "Inactive" : "Active"})
                        .Group(x => new {x.inactive }, g => new { label = g.Key, value = g.Count() })
                        .ToList()
                        .Select(x => new Segment
                        {
                            Label = x.label.inactive,
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
                    chart.Title = "Active/Inactive users";
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

