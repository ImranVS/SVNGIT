using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using VitalSigns.API.Models.Configurator;
using MongoDB.Bson;
using MongoDB.Driver;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq.Expressions;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net.Mail;
using Ionic.Zip;
using Microsoft.AspNet.Authorization;
using System.Web.Security;
using VitalSignsWebSphereDLL;
using VitalSignsLicensing;

namespace VitalSigns.API.Controllers
{

    [Authorize("Bearer", Roles = "Configurator")]
    [Route("[controller]")]
    public class ConfiguratorController : BaseController
    {
        #region Repository declaration
        private IRepository<BusinessHours> businessHoursRepository;
        private IRepository<Credentials> credentialsRepository;
        private IRepository<Location> locationRepository;
        private IRepository<Server> serverRepository;
        private IRepository<ValidLocation> validLocationsRepository;

        private IRepository<Maintenance> maintenanceRepository;

        private IRepository<NameValue> nameValueRepository;


        private IRepository<Server> serversRepository;

        private IRepository<MobileDevices> mobiledevicesRepository;


        private IRepository<Users> maintainUsersRepository;

        private IRepository<TravelerDTS> travelerdatastoreRepository;
        private IRepository<DeviceAttributes> deviceAttributesRepository;

        private IRepository<WindowsService> windowsservicesRepository;

        private IRepository<DominoServerTasks> dominoservertasksRepository;
        private IRepository<ScheduledReports> schedulereportsrepository;
        private IRepository<VSNext.Mongo.Entities.SiteMap> sitemaprepository;
        private IRepository<EventsDetected> eventsdetectedRepository;

        private IRepository<Status> statusRepository;
        private IRepository<Nodes> nodesRepository;
        private IRepository<License> LicenseRepository;

        private IRepository<ServerOther> serverOtherRepository;
        private IRepository<EventsMaster> eventsMasterRepository;
        private IRepository<MobileDevices> mobileDevicesRepository;
        private IRepository<Notifications> notificationsRepository;
        private IRepository<NotificationDestinations> notificationDestRepository;
        private IRepository<Scripts> scriptsRepository;
        private IRepository<Alert_URLs> alertURLsRepository;
        private IRepository<DailyStatistics> dailyStatisticsRepository;
        private IRepository<Database> databaseRepository;
        private IRepository<IbmConnectionsObjects> ibmConnectionsObjectsRepository;
        private IRepository<SharePointWebTrafficDailyStatistics> sharepointWebTrafficDailyStatisticsRepository;
        private IRepository<StatusDetails> statusDetailsRepository;
        private IRepository<SummaryStatistics> summaryStatisticsRepository;
        private IRepository<TravelerStatusSummary> travelerSummaryStatsRepository;
        private IRepository<ServerType> serverTypeRepository;

        VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
        string name;

        LogUtilities.LogUtils logUtils = null;

        #endregion

        #region Application Settings

        #region Preferences
        /// <summary>
        /// Save preferences values
        /// </summary>
        /// <author>Sowmya</author>
        /// <param name="userpreference"></param>
        /// <returns>It returns id value</returns>

        [HttpPut("save_preferences")]
        public APIResponse SavePreferences([FromBody]PreferencesModel userpreference)
        {
            try
            {
                var preferencesSettings = new List<NameValue> { new NameValue { Name = "Company Name", Value = userpreference.CompanyName },
                                                                new NameValue { Name = "Currency Symbol", Value = userpreference.CurrencySymbol },
                                                                new NameValue { Name = "Monitoring Delay", Value = Convert.ToString(userpreference.MonitoringDelay)},
                                                                new NameValue { Name = "Threshold Show", Value =Convert.ToString(userpreference.ThresholdShow)},
                                                                new NameValue { Name = "Dashboard Only", Value = (userpreference.DashboardonlyExecSummaryButtons?"True":"False")},
                                                                new NameValue { Name = "Bing Key", Value = userpreference.BingKey },
                                                                new NameValue { Name = "Purge Interval", Value = userpreference.PurgeInterval }
                                                             };
                var result = Common.SaveNameValues(preferencesSettings);
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), " Settings were successully updated.");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Updating settings has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpPut("save_licence")]
        public APIResponse SaveLicence()
        {
            try
            {
                string licenseKey = Request.Form["licKey"].ToString();
                var preferencesSettings = new List<NameValue> { new NameValue { Name = "Licence Key", Value = licenseKey } };
                var result = Common.SaveNameValues(preferencesSettings);
                VSFramework.TripleDES licKey = new VSFramework.TripleDES();

                string mykey;
                string[] words;
                mykey = licenseKey;
                string Decriptkey = "";
                //Byte[] inputInBytes;
                byte[] Mylicensekey;
                string[] MyEnkey;

                MyEnkey = mykey.Split(',');
                Mylicensekey = new byte[MyEnkey.Length];
                for (int j = 0; j < MyEnkey.Length; j++)
                {
                    Mylicensekey[j] = Byte.Parse(MyEnkey[j]);
                }
                // Decriptkey = VSWebBL.SettingBL.TripleDES.Ins.Decrypt(Mylicensekey);
                Decriptkey = licKey.Decrypt(Mylicensekey);

                if (Decriptkey != null)
                {
                    if (Decriptkey.Contains("#"))
                        words = Decriptkey.Split(new[] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                    else
                        words = null;

                    if (words != null)
                    {
                        License lic = new License();
                        lic.LicenseKey = mykey;
                        lic.units = Convert.ToInt32(words[0]);
                        lic.InstallType = words[1];
                        lic.CompanyName = words[2];
                        lic.LicenseType = words[3];
                        lic.ExpirationDate = DateTime.ParseExact(words[4], "MM/dd/yyyy", null);
                        VSNext.Mongo.Repository.Repository<License> repoLic = new VSNext.Mongo.Repository.Repository<License>(ConnectionString);
                        List<License> licenseList = repoLic.All().ToList();
                        foreach (License l in licenseList)
                            repoLic.Delete(l);

                        repoLic.Insert(lic);
                        VitalSignsLicensing.Licensing licCollection = new VitalSignsLicensing.Licensing();
                        licCollection.refreshServerCollectionWrapper();
                    }
                }
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Licence key saved successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Licence key update has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_preferences")]
        public APIResponse GetPreferences()
        {
            try
            {
                var preferencesSettings = new List<string> { "Company Name", "Currency Symbol", "Monitoring Delay", "Threshold Show", "Dashboard Only", "Bing Key", "Purge Interval" };
                PreferencesModel userpreference = new PreferencesModel();
                var result = Common.GetNameValues(preferencesSettings);
                VSNext.Mongo.Repository.Repository<License> repoLic = new VSNext.Mongo.Repository.Repository<License>(ConnectionString);
                License licenseItem = repoLic.Find(i => i.LicenseKey != "").FirstOrDefault();
                if(result.Exists(x => x.Name == "Company Name"))
                    userpreference.CompanyName = result.FirstOrDefault(x => x.Name == "Company Name").Value;
                if (result.Exists(x => x.Name == "Currency Symbol"))
                    userpreference.CurrencySymbol = result.FirstOrDefault(x => x.Name == "Currency Symbol").Value;
                if (result.Exists(x => x.Name == "Monitoring Delay"))
                    userpreference.MonitoringDelay = Convert.ToInt32(result.FirstOrDefault(x => x.Name == "Monitoring Delay").Value);
                if (result.Exists(x => x.Name == "Threshold Show"))
                    userpreference.ThresholdShow = Convert.ToInt32(result.FirstOrDefault(x => x.Name == "Threshold Show").Value);
                if (result.Exists(x => x.Name == "Dashboard Only"))
                    userpreference.DashboardonlyExecSummaryButtons = Convert.ToBoolean(result.FirstOrDefault(x => x.Name == "Dashboard Only").Value);
                if (result.Exists(x => x.Name == "Bing Key"))
                    userpreference.BingKey = result.FirstOrDefault(x => x.Name == "Bing Key").Value;
                if (result.Exists(x => x.Name == "Purge Interval"))
                    userpreference.PurgeInterval = result.FirstOrDefault(x => x.Name == "Purge Interval").Value;

                Response = Common.CreateResponse(new { userpreference = userpreference, licenseitem = licenseItem });

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), " preferences failed .\n Error Message :" + exception.Message);
            }

            return Response;
        }
        #endregion

        #region Credentials
        /// <summary>
        /// Returns Server Credentials
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>

        [HttpGet("get_credentials")]
        public APIResponse GetAllCredentials()
        {
            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                var result = credentialsRepository.All().Select(x => new ServerCredentialsModel
                {
                    Alias = x.Alias,
                    UserId = x.UserId,
                    DeviceType = x.DeviceType,
                    Id = x.Id,
                    IsModified = false


                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting server credentials has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// Updates Server Credential
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_credentials")]
        public APIResponse UpdateServerCredentials([FromBody]ServerCredentialsModel serverCredential)
        {
            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                Expression<Func<Credentials, bool>> filterExpression;
                byte[] password;
                string bytepwd = "";
                if (!string.IsNullOrEmpty(serverCredential.Password))
                {
                    password = tripleDes.Encrypt(serverCredential.Password);

                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (byte b in password)
                    {
                        stringBuilder.AppendFormat("{0}, ", b);
                    }
                    bytepwd = stringBuilder.ToString();
                    int n = bytepwd.LastIndexOf(", ");
                    bytepwd = bytepwd.Substring(0, n);
                }
                
                if (serverCredential.IsModified)
                {
                    FilterDefinition<Credentials> filterDefination = Builders<Credentials>.Filter.Where(p => p.Id == serverCredential.Id);
                    var updateDefination = credentialsRepository.Updater
                        .Set(p => p.Password, bytepwd);
                    var result = credentialsRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Password updated successfully");
                }
                else
                {
                    if (string.IsNullOrEmpty(serverCredential.Id))
                    {
                        filterExpression = (p => p.Alias == serverCredential.Alias);
                    }
                    else
                    {
                        filterExpression = (p => p.Alias == serverCredential.Alias && p.Id != serverCredential.Id);
                    }
                    var existedData = credentialsRepository.Find(filterExpression).Select(x => x.Alias).FirstOrDefault();
                    if (existedData == null)
                    {
                        if (string.IsNullOrEmpty(serverCredential.Id))
                        {
                            Credentials serverCredentials = new Credentials { Alias = serverCredential.Alias, Password = bytepwd, DeviceType = serverCredential.DeviceType, UserId = serverCredential.UserId };
                            string id = credentialsRepository.Insert(serverCredentials);
                            Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Server credentials inserted successfully");
                        }
                        else
                        {
                            FilterDefinition<Credentials> filterDefination = Builders<Credentials>.Filter.Where(p => p.Id == serverCredential.Id);
                            var updateDefination = credentialsRepository.Updater.Set(p => p.Alias, serverCredential.Alias)
                                .Set(p => p.DeviceType, serverCredential.DeviceType)
                                .Set(p => p.UserId, serverCredential.UserId);
                            var result = credentialsRepository.Update(filterDefination, updateDefination);
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Server credentials updated successfully");
                        }
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                    }
                    else
                    {
                        Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "This alias already exists");
                    }
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving server credentials has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }
        /// <summary>
        /// Delete Server Credential
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete_credential/{Id}")]
        public APIResponse DeleteCredential(string Id)
        {
            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                Expression<Func<Credentials, bool>> expression = (p => p.Id == Id);
                credentialsRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server credentials deleted successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server credentials deletion has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>

        [HttpGet("get_server_types")]

        public APIResponse GetServerTypes()
        {

            try
            {

                serversRepository = new Repository<Server>(ConnectionString);
                var serverTypeData = serversRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.DeviceType, Value = x.DeviceType }).Distinct().ToList().OrderBy(x => x.DisplayText);

                Response = Common.CreateResponse(new { serverTypeData = serverTypeData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

                return Response;
            }
        }
        #endregion

        #region Locations
        /// <summary>
        /// Get Locations Collection
        /// </summary>
        /// <author>Swathi </author>
        /// <param name="country"></param>
        /// <param name="state"></param>
        /// <returns></returns>

        [HttpGet("get_locations")]
        public APIResponse GetLocationsDropDownData(string country, string state)
        {

            try
            {
                validLocationsRepository = new Repository<ValidLocation>(ConnectionString);
                if (string.IsNullOrEmpty(country) && string.IsNullOrEmpty(state))
                {
                    var countryData = validLocationsRepository.All().OrderByDescending(x => x.Country == "United States").Where(x => x.Country != null).Select(x => x.Country).Distinct().ToList();

                    Response = Common.CreateResponse(new { countryData = countryData });
                    // countryData.Insert(0, "-All-");
                }
                if (!string.IsNullOrEmpty(country))
                {
                    var stateData = validLocationsRepository.All().FirstOrDefault(x => x.Country == country).States;
                    // var countryData = validLocationsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.States, Value = x.Id }.OrderBy(x => x.DisplayText);
                    Response = Common.CreateResponse(new { stateData = stateData });

                }

                if (!string.IsNullOrEmpty(country) && !string.IsNullOrEmpty(state))
                {
                    System.Net.WebClient web = new System.Net.WebClient();

                    string cityresponse = web.DownloadString("http://jnitinc.com/WebService/GetCity.php?Country=" + country + "&State=" + state + "");

                    //List<City> ls = deserializeJson<List<LocationValues>>(response);
                    List<CityNames> lst = new List<CityNames>();

                    DataContractJsonSerializer jsonSer = new DataContractJsonSerializer(lst.GetType());
                    System.IO.MemoryStream ms = new System.IO.MemoryStream(System.Text.Encoding.Unicode.GetBytes(cityresponse));
                    lst = (List<CityNames>)jsonSer.ReadObject(ms);
                    // var cities = lst.Select(x => new CityNames
                    //{
                    //    City = x.City,                       
                    //}).ToList();
                    List<string> citynames = new List<string>();
                    foreach (CityNames city in lst)
                    {
                        string cityes = city.City;
                        citynames.Add(city.City);
                    }
                    Response = Common.CreateResponse(new { cityData = citynames });
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

                return Response;
            }
        }
        /// <summary>
        /// All Locations Collection
        /// </summary>
        /// <author>Swathi </author>       
        /// <returns></returns>
        [HttpGet("locations")]
        public APIResponse GetAllLocations()
        {
            locationRepository = new Repository<Location>(ConnectionString);
            var result = locationRepository.All().Select(x => new LocationsModel
            {
                LocationName = x.LocationName,
                Country = x.Country,
                City = x.City,
                Region = x.Region,
                Id = x.Id


            }).ToList();


            Response = Common.CreateResponse(result);
            return Response;


        }

        /// <summary>
        /// Save Locations 
        /// </summary>
        /// <author>Swathi </author>
        /// /// <param name="LocationsModel"></param>
        /// <returns></returns>
        [HttpPut("save_locations")]
        public APIResponse UpdateLocation([FromBody]LocationsModel locations)
        {
            try
            {
                locationRepository = new Repository<Location>(ConnectionString);
                if (string.IsNullOrEmpty(locations.Id))
                {
                    Location location = new Location { LocationName = locations.LocationName, Country = locations.Country, Region = locations.Region, City = locations.City };
                    string id = locationRepository.Insert(location);
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Location inserted successfully");
                }
                else
                {
                    FilterDefinition<Location> filterDefination = Builders<Location>.Filter.Where(p => p.Id == locations.Id);
                    var updateDefination = locationRepository.Updater.Set(p => p.LocationName, locations.LocationName)
                                                             .Set(p => p.City, locations.City)
                                                             .Set(p => p.Country, locations.Country)
                                                             .Set(p => p.Region, locations.Region);
                    var result = locationRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Location updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving location has failed .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        /// Delete Locations 
        /// </summary>
        /// <author>Swathi </author>
        /// /// <param name="Location Id"></param>
        /// <returns></returns>
        [HttpDelete("delete_location/{id}")]
        public APIResponse DeleteLocation(string id)
        {
            try
            {
                locationRepository = new Repository<Location>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                var result = serversRepository.Collection.AsQueryable().Where(x => x.LocationId == id).Select(x=>x.LocationId).FirstOrDefault();
                if(!string.IsNullOrEmpty(result))
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Location is used for different servers. Cannot delete.") ;
                }
                else
                {
                    Expression<Func<Location, bool>> expression = (p => p.Id == id);
                    locationRepository.Delete(expression);
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Location deleted successfully");
                }
               
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of location has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Business Hours
        /// <summary>
        /// Get all business hours data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of business hours details</returns>
        [HttpGet("get_business_hours")]
        public APIResponse GetBusinessHours(bool nameonly = false)
        {
            try
            {
                var result = GetAllBusinessHours(nameonly);
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching business hours failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="nameonly"></param>
        /// <returns></returns>
        public List<dynamic> GetAllBusinessHours(bool nameonly = false)
        {
            List<dynamic> resultList = new List<dynamic>();
            try
            {
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                if (!nameonly)
                {
                    var result = businessHoursRepository.All().Select(x => new BusinessHourModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        StartTime = x.StartTime,
                        Duration = x.Duration,
                        Sunday = x.Days.Contains("Sunday"),
                        Monday = x.Days.Contains("Monday"),
                        Tuesday = x.Days.Contains("Tuesday"),
                        Wednesday = x.Days.Contains("Wednesday"),
                        Thursday = x.Days.Contains("Thursday"),
                        Friday = x.Days.Contains("Friday"),
                        Saturday = x.Days.Contains("Saturday"),
                        UseType = Convert.ToString(x.UseType)
                    }).ToList();
                    resultList.Add(result);
                    return resultList;
                }
                else
                {
                    var result = businessHoursRepository.Find(_ => true)
                        .OrderBy(y => y.Name)
                        .Select(x => x.Name)
                        .ToList();
                    resultList.Add(result);
                    return resultList;
                }
            }
            catch (Exception exception)
            {
                return null;
            }
        }


        /// <summary>
        ///saves the  business hours data
        /// </summary>
        /// <author>Sowjanya</author>

        [HttpPut("save_business_hours")]
        public APIResponse UpdateBusinessHours([FromBody]BusinessHourModel businesshour)
        {
            List<dynamic> business_hours = new List<dynamic>();
            try
            {
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);

                List<string> days = new List<string>();
                if (businesshour.Sunday)
                    days.Add("Sunday");
                if (businesshour.Monday)
                    days.Add("Monday");
                if (businesshour.Tuesday)
                    days.Add("Tuesday");
                if (businesshour.Wednesday)
                    days.Add("Wednesday");
                if (businesshour.Thursday)
                    days.Add("Thursday");
                if (businesshour.Friday)
                    days.Add("Friday");
                if (businesshour.Saturday)
                    days.Add("Saturday");

                Expression<Func<BusinessHours, bool>> filterExpression;
                if (string.IsNullOrEmpty(businesshour.Id))
                {
                    filterExpression = (p => p.Name == businesshour.Name);

                }
                else
                {
                    filterExpression = (p => p.Name == businesshour.Name && p.Id != businesshour.Id);

                }
                var existsData = businessHoursRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();

                if (string.IsNullOrEmpty(existsData))
                {

                    if (string.IsNullOrEmpty(businesshour.Id))
                    {
                        BusinessHours businessHours = new BusinessHours { Name = businesshour.Name, StartTime = businesshour.StartTime, Duration = businesshour.Duration, Days = days.ToArray(), UseType = Convert.ToInt32(businesshour.UseType) };
                        string id = businessHoursRepository.Insert(businessHours);
                        business_hours = GetAllBusinessHours();
                        Response = Common.CreateResponse(business_hours, Common.ResponseStatus.Success.ToDescription(), "Business hours record inserted successfully");
                    }

                    else
                    {
                        FilterDefinition<BusinessHours> filterDefination = Builders<BusinessHours>.Filter.Where(p => p.Id == businesshour.Id);
                        var updateDefination = businessHoursRepository.Updater.Set(p => p.Name, businesshour.Name)
                                                                 .Set(p => p.Duration, businesshour.Duration)
                                                                 .Set(p => p.StartTime, businesshour.StartTime)
                                                                  .Set(p => p.UseType, Convert.ToInt32(businesshour.UseType))
                                                                 .Set(p => p.Days, days.ToArray());

                        var result = businessHoursRepository.Update(filterDefination, updateDefination);
                        business_hours = GetAllBusinessHours();
                        Response = Common.CreateResponse(business_hours, Common.ResponseStatus.Success.ToDescription(), "Business hours record updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "This name already exists. Please enter another one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving business hours has failed. \n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///delete the  business hours data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpDelete("delete_business_hours/{id}")]
        public APIResponse DeleteBusinessHours(string id)
        {
            try
            {
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                Expression<Func<BusinessHours, bool>> expression = (p => p.Id == id);
                businessHoursRepository.Delete(expression);
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Business hours deleted successfully");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of business hours has failed .\n Error Message :" + exception.Message);
            }

            return Response;

        }

        #endregion

        #region Maintainance
        [HttpGet("get_maintenance")]
        /// <summary>
        /// Get all maintenance data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of maintenance data</returns>
        public APIResponse GetAllMaintenance()
        {
            List<dynamic> result = new List<dynamic>();

            try
            {
                result = GetMaintenanceList();
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Maintenance record inserted successfully");
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching maintenance information has failed .\n Error Message :" + ex.Message);
            }
            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>

        public List<dynamic> GetMaintenanceList()
        {
            List<dynamic> result_disp = new List<dynamic>();
            List<string> serverList = new List<string>();
            List<string> userList = new List<string>();
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                var maintainWindows = maintenanceRepository.All().ToList();

                serversRepository = new Repository<Server>(ConnectionString);
                var servers = serversRepository.Collection.AsQueryable().Select(x => new { ServerID = x.Id, MaintenanceWindows = x.MaintenanceWindows });
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var keyUsers = mobileDevicesRepository.Collection.AsQueryable()
                    .Where(x => x.ThresholdSyncTime != null)
                    .Select(x => new { KeyuserID = x.Id, MaintenanceWindows = x.MaintenanceWindows });
                foreach (var maintainWindow in maintainWindows)
                {
                    serverList = new List<string>();
                    var innerServers = servers.Where(x => x.MaintenanceWindows.Contains(maintainWindow.Id)).ToList();
                    foreach (var server in innerServers)
                    {
                        serverList.Add(server.ServerID);
                    }
                    userList = new List<string>();
                    var innerkeyUsers = keyUsers.Where(x => x.MaintenanceWindows.Contains(maintainWindow.Id)).ToList();
                    foreach (var keyUser in innerkeyUsers)
                    {
                        userList.Add(keyUser.KeyuserID);
                    }

                    result_disp.Add(new MaintenanceModel
                    {
                        Id = maintainWindow.Id,
                        Name = maintainWindow.Name,
                        StartDate = maintainWindow.StartDate,
                        StartTime = maintainWindow.StartTime,
                        EndDate = maintainWindow.EndDate,
                        EndTime = maintainWindow.EndTime,
                        Duration = maintainWindow.Duration,
                        DurationType = Convert.ToString(maintainWindow.DurationType),

                        MaintenanceDaysList = maintainWindow.MaintenanceDaysList,
                        ContinueForever = maintainWindow.ContinueForever,
                        MaintainType = Convert.ToString(maintainWindow.MaintainType) == "1" ? "One Time" :
                        Convert.ToString(maintainWindow.MaintainType) == "2" ? "Daily" :
                        Convert.ToString(maintainWindow.MaintainType) == "3" ? "Weekly" :
                        Convert.ToString(maintainWindow.MaintainType) == "4" ? "Monthly" : "-",

                        MaintainTypeValue = Convert.ToString(maintainWindow.MaintainType),
                        KeyUsers = userList,
                        DeviceList = serverList

                    });
                }

                return result_disp;
            }
            catch (Exception exception)
            {
                return result_disp;
            }

        }
        /// <summary>
        ///saves the  maintenance data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpPut("save_maintenancedata")]
        public APIResponse UpdateMaintenancedata([FromBody]MaintenanceModel maintenance)
        {
            List<string> tempList = new List<string>();
            List<dynamic> result_disp = new List<dynamic>();
            FilterDefinition<Server> filterServerDef;
            FilterDefinition<MobileDevices> filterUserDef;
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                Expression<Func<Maintenance, bool>> filterExpression;
                if (string.IsNullOrEmpty(maintenance.Id))
                {
                    filterExpression = (p => p.Name == maintenance.Name);
                }
                else
                {
                    filterExpression = (p => p.Name == maintenance.Name && p.Id != maintenance.Id);

                }
                var existsData = maintenanceRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();

                if (string.IsNullOrEmpty(existsData))
                {
                    if (string.IsNullOrEmpty(maintenance.Id))
                    {
                        Maintenance maintenancedata = new Maintenance
                        {
                            Name = maintenance.Name,
                            StartDate = maintenance.StartDate,
                            StartTime = maintenance.StartTime,
                            Duration = maintenance.Duration,
                            EndDate = maintenance.EndDate,
                            EndTime = maintenance.EndTime,
                            MaintenanceDaysList = maintenance.MaintenanceDaysList,
                            MaintainType = maintenance.MaintainType == "" ? 0 : Convert.ToInt32(maintenance.MaintainType),
                            DurationType = maintenance.DurationType == "" ? 0 : Convert.ToInt32(maintenance.DurationType)
                        };
                        maintenance.Id = maintenanceRepository.Insert(maintenancedata);

                        serversRepository = new Repository<Server>(ConnectionString);
                        UpdateDefinition<Server> updateDefinition = null;
                        var devicesList = maintenance.DeviceList;
                        foreach (string id in devicesList)
                        {
                            var server = serversRepository.Get(id);
                            if (server.MaintenanceWindows != null)
                            {
                                if (!server.MaintenanceWindows.Contains(maintenance.Id))
                                {
                                    server.MaintenanceWindows.Add(maintenance.Id);
                                    updateDefinition = serversRepository.Updater.Set(p => p.MaintenanceWindows, server.MaintenanceWindows);
                                    var result = serversRepository.Update(server, updateDefinition);
                                }
                            }
                            else
                            {
                                List<string> maintainanceWindow = new List<string>();
                                maintainanceWindow.Add(maintenance.Id);
                                updateDefinition = serversRepository.Updater.Set(p => p.MaintenanceWindows, maintainanceWindow);
                                var result = serversRepository.Update(server, updateDefinition);
                            }
                        }

                        mobiledevicesRepository = new Repository<MobileDevices>(ConnectionString);
                        UpdateDefinition<MobileDevices> mupdateDefinition = null;

                        var keyusersList = maintenance.KeyUsers;
                        foreach (string id in keyusersList)
                        {
                            var keyUser = mobiledevicesRepository.Get(id);
                            if (keyUser.MaintenanceWindows != null)
                            {
                                if (!keyUser.MaintenanceWindows.Contains(maintenance.Id))
                                {
                                    keyUser.MaintenanceWindows.Add(maintenance.Id);
                                    mupdateDefinition = mobiledevicesRepository.Updater.Set(p => p.MaintenanceWindows, keyUser.MaintenanceWindows);
                                    var result = mobiledevicesRepository.Update(keyUser, mupdateDefinition);
                                }
                            }
                            else
                            {
                                List<string> maintainanceWindow = new List<string>();
                                maintainanceWindow.Add(maintenance.Id);
                                mupdateDefinition = mobiledevicesRepository.Updater.Set(p => p.MaintenanceWindows, maintainanceWindow);
                                var result = mobiledevicesRepository.Update(keyUser, mupdateDefinition);
                            }
                        }
                        result_disp = GetMaintenanceList();
                        Response = Common.CreateResponse(result_disp, Common.ResponseStatus.Success.ToDescription(), "Maintenance data inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<Maintenance> filterDefination = Builders<Maintenance>.Filter.Where(p => p.Id == maintenance.Id);
                        var updateDefination = maintenanceRepository.Updater.Set(p => p.Name, maintenance.Name)
                            .Set(p => p.StartDate, maintenance.StartDate)
                            .Set(p => p.StartTime, maintenance.StartTime)
                            .Set(p => p.Duration, maintenance.Duration)
                            .Set(p => p.EndDate, maintenance.EndDate)
                            .Set(p => p.EndTime, maintenance.EndTime)
                            .Set(p => p.MaintainType, maintenance.MaintainType == "" ? 0 : Convert.ToInt32(maintenance.MaintainType))
                            .Set(p => p.DurationType, maintenance.DurationType == "" ? 0 : Convert.ToInt32(maintenance.DurationType))
                            .Set(p => p.MaintenanceDaysList, maintenance.MaintenanceDaysList);
                        var result = maintenanceRepository.Update(filterDefination, updateDefination);

                        serversRepository = new Repository<Server>(ConnectionString);
                        filterServerDef = serversRepository.Filter.AnyEq(x => x.MaintenanceWindows, maintenance.Id);
                        var serversList = serversRepository.Find(filterServerDef).ToList();

                        if (serversList.Count > 0)
                        {
                            foreach (var serverDef in serversList)
                            {
                                if (!maintenance.DeviceList.Contains(serverDef.Id))
                                {
                                    if (serverDef.MaintenanceWindows != null)
                                    {
                                        var maintList = serverDef.MaintenanceWindows.ToList();
                                        var itemToRemove = maintList.Single(r => r == maintenance.Id);
                                        serverDef.MaintenanceWindows.Remove(itemToRemove);
                                        serversRepository.Replace(serverDef);
                                    }

                                }
                            }
                        }

                        filterServerDef = serversRepository.Filter.In(x => x.Id, maintenance.DeviceList);
                        serversList = serversRepository.Find(filterServerDef).ToList();

                        if (serversList.Count > 0)
                        {
                            foreach (var serverDef in serversList)
                            {
                                tempList = new List<string>();
                                if (serverDef.MaintenanceWindows != null)
                                {
                                    var maintList = serverDef.MaintenanceWindows.ToList();
                                    int ind = maintList.FindIndex(x => x == maintenance.Id);
                                    if (ind < 0)
                                    {
                                        serverDef.MaintenanceWindows.Add(maintenance.Id);
                                    }
                                }
                                else
                                {
                                    tempList.Add(maintenance.Id);
                                    serverDef.MaintenanceWindows = tempList;
                                }
                                serversRepository.Replace(serverDef);
                            }
                        }


                        mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
                        filterUserDef = mobileDevicesRepository.Filter.AnyEq(x => x.MaintenanceWindows, maintenance.Id);
                        var userList = mobileDevicesRepository.Find(filterUserDef).ToList();

                        if (userList.Count > 0)
                        {
                            foreach (var userDef in userList)
                            {
                                if (!maintenance.KeyUsers.Contains(userDef.Id))
                                {
                                    if (userDef.MaintenanceWindows != null)
                                    {
                                        var maintList = userDef.MaintenanceWindows.ToList();
                                        var itemToRemove = maintList.Single(r => r == maintenance.Id);
                                        userDef.MaintenanceWindows.Remove(itemToRemove);
                                        mobileDevicesRepository.Replace(userDef);
                                    }

                                }
                            }
                        }

                        filterUserDef = mobileDevicesRepository.Filter.In(x => x.Id, maintenance.KeyUsers);
                        userList = mobileDevicesRepository.Find(filterUserDef).ToList();

                        if (userList.Count > 0)
                        {
                            foreach (var userDef in userList)
                            {
                                tempList = new List<string>();
                                if (userDef.MaintenanceWindows != null)
                                {
                                    var maintList = userDef.MaintenanceWindows.ToList();
                                    int ind = maintList.FindIndex(x => x == maintenance.Id);
                                    if (ind < 0)
                                    {
                                        userDef.MaintenanceWindows.Add(maintenance.Id);
                                    }
                                }
                                else
                                {
                                    tempList.Add(maintenance.Id);
                                    userDef.MaintenanceWindows = tempList;
                                }
                                mobileDevicesRepository.Replace(userDef);
                            }
                        }
                        result_disp = GetMaintenanceList();
                        Response = Common.CreateResponse(result_disp, Common.ResponseStatus.Success.ToDescription(), "Maintenance record updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This name already exists. Please enter another one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving of a maintenance record has failed .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        ///delete the maintenance data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpDelete("delete_maintenancedata/{id}")]
        public APIResponse DeleteMaintenancedata(string id)
        {
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                Expression<Func<Maintenance, bool>> expression = (p => p.Id == id);
                maintenanceRepository.Delete(expression);
                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Maintenance record deleted succesfully");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of a maintenance record has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        ///get the maintenance data for configurator
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpGet("get_server_maintenancedata")]
        public APIResponse GetServerMaintenanceData(string id, string fromDate, string fromTime, string toDate, string toTime)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            maintenanceRepository = new Repository<Maintenance>(ConnectionString);
            List<MaintenanceModel> maintenanceWindows = new List<MaintenanceModel>();

            //Expression<Func<Server, bool>> attributeexpression = (p => p.Id == id);

            // var result = serversRepository.Find(attributeexpression).Select(x => x.MaintenanceWindows).FirstOrDefault();

            //var results = maintenanceRepository.Collection.AsQueryable().Where(s => result.Contains(s.Id) && (!string.IsNullOrEmpty(fromdate) ? s.StartDate == Convert.ToDateTime(fromdate) : true) && (!string.IsNullOrEmpty(todate) ? s.EndDate == Convert.ToDateTime(todate) : true)
            //&& (!string.IsNullOrEmpty(fromdate) && !string.IsNullOrEmpty(todate) && !string.IsNullOrEmpty(totime) && !string.IsNullOrEmpty(fromtime) ?
            //s.StartDate >= Convert.ToDateTime(fromdate) && s.EndDate <= Convert.ToDateTime(todate) : true))

            try
            {
                Expression<Func<Server, bool>> attributeexpression = (p => p.Id == id);
                var serverResult = serversRepository.Find(attributeexpression).ToList();

                List<dynamic> finalResult = new List<dynamic>();

                var result = serversRepository.Find(attributeexpression).Select(x => x.MaintenanceWindows).FirstOrDefault();

                if (!string.IsNullOrEmpty(fromDate) && !string.IsNullOrEmpty(toDate) && !string.IsNullOrEmpty(fromTime) && !string.IsNullOrEmpty(toTime))
                {
                    var results = maintenanceRepository.Collection.AsQueryable().Where(s => result.Contains(s.Id) && s.StartDate == Convert.ToDateTime(fromDate))
                                                                   .Select(m => new
                                                                   {
                                                                       id = m.Id,
                                                                       Name = m.Name,
                                                                       StartDate = m.StartDate,
                                                                       StartTime = m.StartTime,
                                                                       EndDate = m.EndDate,
                                                                       Duration = m.Duration,
                                                                       MaintainType = m.MaintainType == 1 ? "One Time" :
                                                                                      m.MaintainType == 2 ? "Daily" :
                                                                                      m.MaintainType == 3 ? "Weekly" :
                                                                                      m.MaintainType == 4 ? "Monthly" : "-",
                                                                       MaintenanceFrequency = m.MaintenanceFrequency,
                                                                       MaintenanceDaysList = m.MaintenanceDaysList
                                                                   }).ToList();

                    foreach (var serverItem in serverResult)
                    {
                        foreach (var m in results)
                        {
                            finalResult.Add(new
                            {
                                id = m.id,
                                Name = m.Name,
                                StartDate = m.StartDate,
                                StartTime = m.StartTime,
                                EndDate = m.EndDate,
                                Duration = m.Duration,
                                MaintainType = m.MaintainType,
                                MaintenanceFrequency = m.MaintenanceFrequency,
                                MaintenanceDaysList = m.MaintenanceDaysList,
                                DeviceType = serverItem.DeviceType,
                                DeviceName = serverItem.DeviceName
                            });
                        }
                    }
                }
                else
                {
                    var results = maintenanceRepository.Collection.AsQueryable().Where(s => result.Contains(s.Id))
                                                            .Select(m => new
                                                            {
                                                                id = m.Id,
                                                                Name = m.Name,
                                                                StartDate = m.StartDate,
                                                                StartTime = m.StartTime,
                                                                EndDate = m.EndDate,
                                                                Duration = m.Duration,
                                                                MaintainType = m.MaintainType == 1 ? "One Time" :
                                                                               m.MaintainType == 2 ? "Daily" :
                                                                               m.MaintainType == 3 ? "Weekly" :
                                                                               m.MaintainType == 4 ? "Monthly" : "-",
                                                                MaintenanceFrequency = m.MaintenanceFrequency,
                                                                MaintenanceDaysList = m.MaintenanceDaysList

                                                            }).ToList();

                    foreach (var serverItem in serverResult)
                    {
                        foreach (var m in results)
                        {
                            finalResult.Add(new
                            {
                                id = m.id,
                                Name = m.Name,
                                StartDate = m.StartDate,
                                StartTime = m.StartTime,
                                EndDate = m.EndDate,
                                Duration = m.Duration,
                                MaintainType = m.MaintainType,
                                MaintenanceFrequency = m.MaintenanceFrequency,
                                MaintenanceDaysList = m.MaintenanceDaysList,
                                DeviceType = serverItem.DeviceType,
                                DeviceName = serverItem.DeviceName
                            });
                        }
                    }
                }


                Response = Common.CreateResponse(finalResult);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

            }
            return Response;
        }

        #endregion

        #region Users
        /// <summary>
        /// 
        /// </summary>
        /// <author>Sowmya</author>
        /// <returns></returns>

        [HttpGet("get_maintain_users")]
        public APIResponse GetAllMaintainUsers()
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);
                var result = maintainUsersRepository.All().Select(x => new MaintainUsersModel
                {
                    Id = x.Id,
                    Email = x.Email,
                    FullName = x.FullName,
                    Roles = x.Roles,
                    Status = x.Status,
                }).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting user information has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="maintainuser"></param>
        /// <returns></returns>

        [HttpPut("save_maintain_users")]
        public APIResponse UpdateMaintainUsers([FromBody]MaintainUsersModel maintainuser)
        {
            try
            {
                //5/3/2017 NS modified for VSPLUS-3566 - the second parameter is TenantId; hard-coded to 5 for now
                maintainUsersRepository = new Repository<Users>(ConnectionString, 5);
                Expression<Func<Users, bool>> filterExpression;
                if (string.IsNullOrEmpty(maintainuser.Id))
                {
                    filterExpression = (p => p.Email == maintainuser.Email);

                }
                else
                {
                    filterExpression = (p => p.Email == maintainuser.Email && p.Id != maintainuser.Id);

                }
                var existsData = maintainUsersRepository.Find(filterExpression).Select(x => x.Email).FirstOrDefault();
                if (string.IsNullOrEmpty(existsData))
                {
                    if (string.IsNullOrEmpty(maintainuser.Id))
                    {
                        Users maintainUsers = new Users { FullName = maintainuser.FullName, Email = maintainuser.Email, Roles = maintainuser.Roles, Status = maintainuser.Status };
                        string password = Membership.GeneratePassword(6, 2);
                        string hashedPassword = Startup.SignData(password);
                        maintainUsers.Hash = hashedPassword;
                        string id = maintainUsersRepository.Insert(maintainUsers);
                        (new Common()).SendPasswordEmail(maintainuser.Email, password);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "User information inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<Users> filterDefination = Builders<Users>.Filter.Where(p => p.Id == maintainuser.Id);
                        var updateDefination = maintainUsersRepository.Updater.Set(p => p.FullName, maintainuser.FullName)
                                                                 .Set(p => p.Email, maintainuser.Email)
                                                                 .Set(p => p.Status, maintainuser.Status)
                                                                 .Set(p => p.Roles, maintainuser.Roles);
                        var result = maintainUsersRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "User information updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This e-mail (" + maintainuser.Email + ") already exists. Please enter a different one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving user information has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_maintain_users/{id}")]
        public APIResponse DeleteMaintainUsers(string id)
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);
                Expression<Func<Users, bool>> expression = (p => p.Id == id);
                maintainUsersRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "User information deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of user information has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }




        #endregion

        #endregion

        #region Device Settings
        #region Device Attributes
        /// <summary>
        /// Get all device attributes data
        /// </summary>
        /// <author>Swathi</author>
        /// <returns>List of device attributes data</returns>
        [HttpGet("get_device_attributes")]
        public APIResponse GetDeviceAttributes(string type)

        {
            try
            {
               
                Response = Common.CreateResponse(GetDeviceAttributeList(type));
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting device attributes has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        public List<DeviceAttributesModel> GetDeviceAttributeList( string type)
        {
            deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
            var result = deviceAttributesRepository.Find(x => x.DeviceType == type).Select(x => new DeviceAttributesModel
            {
                Id = x.Id,
                AttributeName = x.AttributeName,
                DefaultValue = x.DefaultValue,
                DeviceType = x.DeviceType,
                FieldName = x.FieldName,
                DataType = x.DataType,
                Unitofmeasurement = x.Unitofmeasurement,
                Category = x.Category,
                IsSelected = false,
                IsPercentage = x.IsPercentage,
                Type = x.Type
            }).OrderBy(x => x.AttributeName).ToList();
            return result;
        }
        /// <summary>
        ///saves the device attributes data
        /// </summary>
        /// <author>Swathi</author>
        [HttpPut("save_device_attributes")]
        public APIResponse SaveDeviceAttributes([FromBody]DeviceSettings deviceSettings)
        {
            try
            {
                var deviceAttributes = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Value).ToObject<List<DeviceAttributeValue>>();

                var devicesList = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Devices).ToObject<string[]>();
                Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                UpdateDefinition<BsonDocument> updateDefinition = null;
                if (deviceAttributes.Count() == 0)
                {
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Error.ToDescription(), "Please select at least one attribute.");
                }
                else
                {
                    foreach (string id in devicesList)
                    {
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                        foreach (var attribute in deviceAttributes)
                        {
                            if (!string.IsNullOrEmpty(attribute.FieldName))
                            {

                                //updateDefinition = Builders<BsonDocument>.Update
                                //   .Set(attribute.FieldName, attribute.Value);
                                string field = attribute.FieldName;
                                string value = attribute.Value;
                                bool defaultvalues = attribute.DefaultboolValues;
                                string datatype = attribute.DataType;
                                if (datatype == "int")
                                {
                                    int outputvalue = Convert.ToInt32(value);
                                    updateDefinition = Builders<BsonDocument>.Update
                                          .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "double")
                                {
                                    double outputvalue = Convert.ToDouble(value);
                                    updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "bool")
                                {
                                    bool booloutput;
                                    if (value == "0")
                                    {
                                        booloutput = false;
                                    }
                                    else
                                    {
                                        booloutput = true;
                                    }
                                    updateDefinition = Builders<BsonDocument>.Update
                                                                                                        .Set(field, booloutput);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }


                                if (datatype == "string")
                                {
                                    updateDefinition = Builders<BsonDocument>.Update
                                                                                                       .Set(field, value);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                //  Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server Attributes Updated Successfully.");
                            }
                        }
                    }
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server attributes updated successfully");
                }
                // var result = repository.Collection.UpdateMany(filter, updateDefinition);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server attributes update has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Domino Server Tasks
        /// <summary>
        /// Get all domino server tasks data
        /// </summary>
        /// <author>Sowjanya</author>
        /// <returns>List of domino servertasks data</returns>

        [HttpGet("get_domino_server_tasks")]
        public APIResponse GetAllDominoServerTasks()
        {
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                var result = dominoservertasksRepository.All().Select(x => new DominoServerTasksModel
                {
                    Id = x.Id,
                    IsSelected = false,
                    TaskName = x.TaskName,
                    IsLoad = false,
                    IsRestartASAP = false,
                    IsResartLater = false,
                    IsDisallow = false
                }).OrderBy(x => x.TaskName).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting server tasks has failed .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        ///saves the domino server tasks  data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpPut("save_domino_server_tasks")]
        public APIResponse SaveDominoServerTasks([FromBody]DeviceSettings dominoserversettings)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                string setting = Convert.ToString(dominoserversettings.Setting);
                var selectedServerTasks = ((Newtonsoft.Json.Linq.JArray)dominoserversettings.Value).ToObject<List<DominoServerTasksValue>>();
                var devicesList = ((Newtonsoft.Json.Linq.JArray)dominoserversettings.Devices).ToObject<string[]>();
                UpdateDefinition<Server> updateDefinition = null;


                if (devicesList.Count() > 0 && selectedServerTasks.Count() > 0 && !string.IsNullOrEmpty(setting.Trim()))
                {

                    foreach (string id in devicesList)
                    {
                        var server = serversRepository.Get(id);
                        List<DominoServerTask> dominoServerTasks = new List<DominoServerTask>();
                        dominoServerTasks.AddRange(server.ServerTasks);
                        name = string.Empty;

                        foreach (var serverTask in selectedServerTasks)
                        {
                            Expression<Func<Server, bool>> filterExpression1 = (p => p.Id == id);
                            var existsData = serversRepository.Find(filterExpression1).Select(x => x.ServerTasks).ToList();
                            foreach (var data in existsData)
                            {
                                foreach (var nameData in data)
                                {

                                    if (nameData.TaskName == serverTask.TaskName)
                                    {
                                        name = "exists";
                                    }

                                }
                            }


                            if (name == "exists")
                            {
                                Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "One of the selected tasks is already assigned to the server(s). Please select another one.");
                            }
                            else
                            {

                                if (setting.Equals("add"))
                                {
                                    DominoServerTask dominoServerTask = new DominoServerTask();
                                    dominoServerTask.Id = ObjectId.GenerateNewId().ToString();
                                    dominoServerTask.TaskId = serverTask.Id;
                                    dominoServerTask.TaskName = serverTask.TaskName;
                                    dominoServerTask.SendLoadCmd = serverTask.IsLoad;
                                    dominoServerTask.Monitored = true;
                                    dominoServerTask.SendRestartCmd = serverTask.IsResartLater;
                                    dominoServerTask.SendRestartCmdOffhours = serverTask.IsRestartASAP;
                                    dominoServerTask.SendExitCmd = serverTask.IsDisallow;
                                    dominoServerTasks.Add(dominoServerTask);
                                    //2/24/2017 NS added for VSPLUS-3506
                                    Licensing licensing = new Licensing();
                                    licensing.refreshServerCollectionWrapper();
                                    Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Domino server tasks added successfully");
                                }
                            }
                            if (setting.Equals("remove"))
                            {
                                var dominoServerTaskRemove = dominoServerTasks.Where(x => x.TaskId == serverTask.Id).ToList();
                                foreach (var item in dominoServerTaskRemove)
                                    dominoServerTasks.Remove(item);
                                //2/24/2017 NS added for VSPLUS-3506
                                Licensing licensing = new Licensing();
                                licensing.refreshServerCollectionWrapper();
                                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Domino server tasks removed successfully");
                            }

                        }
                        updateDefinition = serversRepository.Updater.Set(p => p.ServerTasks, dominoServerTasks);
                        var result = serversRepository.Update(server, updateDefinition);
                    }
                    // Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Settings are not selected");
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "No devices were selected");
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Adding/removing Domino server task(s) has failed. \n Error Message :" + exception.Message);
            }
            return Response;
        }



        #endregion

        #region Windows Services
        /// <summary>
        /// 
        /// </summary>
        /// <author>Swathi </author>
        /// <param name=""></param>
        /// <param name=""></param>
        /// <returns></returns>
        [HttpGet("get_windows_services")]
        public APIResponse GetAllWindowservices(string deviceId)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var serverName = serversRepository.Find(x => x.Id == deviceId).ToList().First();
                if (serverName.WindowServices == null)
                {
                    Response = Common.CreateResponse("No Windows Services availabe");
                }
                else
                { 
                    var result = serverName
                    .WindowServices
                    .Select(x => new WindowsServiceModel
                    {
                        ServiceName = x.ServiceName,
                        ServerRequired = x.ServerRequired,
                        Monitored = x.Monitored,
                        Status = x.Status,
                        StartupMode = x.StartupMode,
                        DisplayName = x.DisplayName
                    }).OrderByDescending(x => x.ServerRequired)
                    .ThenByDescending(x => x.Monitored)
                    .ThenBy(x => x.DisplayName)
                    .ToList();
                Response = Common.CreateResponse(result);
            }
               }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        #endregion

        #region Disk Settings
        /// <summary>
        /// Returns Disk Names
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_disk_names")]
        public APIResponse GetStatusOfServerDiskDrives()

        {

            try
            {
                statusRepository = new Repository<Status>(ConnectionString);


                var disks = statusRepository.Collection.AsQueryable().Select(x => x.Disks).ToList();


                SelectedDiksModel serverDiskStatus = new SelectedDiksModel();


                List<string> diskNames = new List<string>();
                foreach (List<DiskStatus> drive in disks)
                {
                    if (drive != null)
                        diskNames.AddRange(drive.Select(x => x.DiskName));
                }
                // var result = serverDiskStatus.Drives.Select(x => x.DiskName).Distinct().ToList();
                List<SelectedDiksModel> drives = new List<SelectedDiksModel>();
                foreach (var name in diskNames.Distinct())
                {
                    SelectedDiksModel drive = new SelectedDiksModel();
                    //  drive.IsSelected=false;
                    drive.DiskName = name;
                    drive.FreespaceThreshold = "";
                    drives.Add(drive);
                }

                Response = Common.CreateResponse(drives);
            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error in getting disk names");
            }



            return Response;
        }
        /// <summary>
        /// Updates Disk Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_disk_settings")]
        public APIResponse SaveDiskSettings([FromBody]DeviceSettings deviceSettings)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {

                string setting = Convert.ToString(deviceSettings.Setting);
                string settingValue = Convert.ToString(deviceSettings.Value);
                var devicesList = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Devices).ToObject<string[]>();
                UpdateDefinition<Server> updateDefinition = null;
                if (devicesList.Count() > 0 && !string.IsNullOrEmpty(setting))
                {

                    foreach (string id in devicesList)
                    {
                        var server = serversRepository.Get(id);
                        List<DiskSetting> diskSettings = new List<DiskSetting>();

                        if (setting.Equals("allDisksBypercentage"))
                            diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "Percent" });
                        else if (setting.Equals("allDisksByGB"))
                            diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "GB" });
                        else if (setting.Equals("noDiskAlerts"))
                            diskSettings.Add(new DiskSetting { DiskName = "NoAlerts", Threshold = null, ThresholdType = null });
                        else if (setting.Equals("selectedDisks"))
                        {
                            List<SelectedDiksModel> selectedDisks = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Value).ToObject<List<SelectedDiksModel>>();
                            foreach (var item in selectedDisks)
                            {
                                if (item.IsSelected)
                                {
                                    diskSettings.Add(new DiskSetting {
                                        DiskName = item.DiskName,
                                        Threshold = item.ThresholdType == "Percent" ? Convert.ToDouble(item.FreespaceThreshold) / 100 : Convert.ToDouble(item.FreespaceThreshold),
                                        ThresholdType = item.ThresholdType });
                                }
                            }
                        }
                        if (diskSettings.Count > 0)
                        {
                            updateDefinition = serversRepository.Updater.Set(p => p.DiskInfo, diskSettings);
                            var result = serversRepository.Update(server, updateDefinition);
                        }

                    }
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Disk settings sucessfully updated");
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "No devices were selected");
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving server disk settings has failed. " + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Location/Credentials/Business Hours
        /// <summary>
        /// Returns Locations\Credentials\Business Hours
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_server_credentials_businesshours")]

        public APIResponse GetDeviceListDropDownData()
        {

            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                serverTypeRepository = new Repository<ServerType>(ConnectionString);

                var serverTypeData = Common.GetServerTypes();
               

                var credentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var businessHoursData = businessHoursRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var locationsData = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                Response = Common.CreateResponse(new { credentialsData = credentialsData, businessHoursData = businessHoursData, locationsData = locationsData, serverTypeData = serverTypeData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);

                return Response;
            }
        }
        /// <summary>
        /// Updates SSE Locations\Credentials\Business Hours
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_server_credentials_businesshours")]
        public APIResponse UpdateServerCredentialsBusinessHours([FromBody]DeviceSettings deviceSettings)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                string setting = Convert.ToString(deviceSettings.Setting);
                string settingValue = Convert.ToString(deviceSettings.Value);
                var devicesList = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Devices).ToObject<string[]>();
                UpdateDefinition<Server> updateDefinition = null;

                if (devicesList.Count() > 0 && !string.IsNullOrEmpty(setting) && !string.IsNullOrEmpty(settingValue))
                {
                    //var devicesList = devices.Replace('[',' ').Replace(']', ' ').Trim().Split(',');

                    foreach (string id in devicesList)
                    {

                        FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                        if (setting.Equals("locations"))
                            updateDefinition = serversRepository.Updater.Set(p => p.LocationId, settingValue);
                        else if (setting.Equals("credentials"))
                            updateDefinition = serversRepository.Updater.Set(p => p.CredentialsId, settingValue);
                        else if (setting.Equals("businessHours"))

                            updateDefinition = serversRepository.Updater.Set(p => p.BusinessHoursId, settingValue);
                        if (updateDefinition != null)
                        {
                            var result = serversRepository.Update(filterDefination, updateDefinition);
                            //2/24/2017 NS added for VSPLUS-3506
                            Licensing licensing = new Licensing();
                            licensing.refreshServerCollectionWrapper();
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Settings updated successfully");
                        }
                        else
                        {
                            Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Settings were not updated");
                        }
                    }
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "No devices were selected");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving the settings has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        #endregion

        #endregion

        #region Servers

        /// <summary>
        /// Get all Server attributes data
        /// </summary>
        /// <author>Swathi</author>
        /// <returns>List of Server attributes data</returns>
        [HttpGet("{id}/servers_attributes")]
        public APIResponse GetAllServersAttributes(string id)
        {
            try
            {



                Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                serversRepository = new Repository<Server>(ConnectionString);
                deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                Expression<Func<Server, bool>> attributeexpression = (p => p.Id == id);
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var result = repository.Collection.Find(filter).FirstOrDefault();

                var serverresult = serversRepository.Find(attributeexpression).AsQueryable().Select(s => new DeviceAttributesDataModel
                {
                    //CellName=s.CellName,
                    //NodeName=s.NodeName,
                    DeviceName = s.DeviceName,
                    Description = s.Description,
                    IPAddress = s.IPAddress,
                    Category = s.Category,
                    IsEnabled = s.IsEnabled,
                    Platform = s.Platform,
                    LocationId = s.LocationId,
                    Devicetype = s.DeviceType,
                    CellId = s.CellId,
                    NodeId = s.NodeId,
                    CredentialsId = s.CredentialsId,
                    Mode = s.Mode,
                    RequireSSL = s.RequireSSL,
                    AuthenticationType = s.AuthenticationType,
                    HostName = s.HostName,
                    PortName = s.PortName

                    // CellName = serversRepository.Collection.Find(filter).AsQueryable().Select(x => x.NodeId).FirstOrDefault(),
                    //   NodeName = serversRepository.Collection.AsQueryable().Select(x => x.NodeIds).FirstOrDefault()


                }).FirstOrDefault();
                if (!string.IsNullOrEmpty(serverresult.LocationId))
                {
                    var locationname = locationRepository.All().Where(x => x.Id == serverresult.LocationId).Select(x => new Location
                    {
                        LocationName = x.LocationName


                    }).FirstOrDefault();
                    serverresult.LocationId = locationname.LocationName;
                }
                if (serverresult.Devicetype == "WebSphere")
                {
                    var cellname = serversRepository.Collection.AsQueryable().Where(x => x.Id == serverresult.CellId).Select(x => new DeviceAttributesDataModel
                    {
                        CellName = x.DeviceName


                    }).FirstOrDefault();
                    serverresult.CellName = cellname.CellName;
                    var nodename = serversRepository.Collection.AsQueryable().Where(x => x.Id == serverresult.NodeId).Select(x => new DeviceAttributesDataModel
                    {
                        NodeName = x.DeviceName


                    }).FirstOrDefault();
                    serverresult.NodeName = nodename.NodeName;
                }
                // var credentialsData = credentialsRepository.All().Where(x => x.DeviceType == serverresult.Devicetype).Select(x => x.Alias).Distinct().OrderBy(x => x).ToList();
                
                var credentialsData = credentialsRepository.Collection.AsQueryable().Where(x => x.DeviceType == serverresult.Devicetype).Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);


                Expression<Func<DeviceAttributes, bool>> attributesexpression = (p => p.DeviceType == serverresult.Devicetype);
                List<DeviceAttributesModel> deviceAttributes = new List<DeviceAttributesModel>();

               var attributes = deviceAttributesRepository.Collection.Find(attributesexpression).ToList().OrderBy(x => x.Category).Select(x => new DeviceAttributesModel
                {
                    AttributeName = x.AttributeName,
                    Category = x.Category,
                    Type = x.Type,
                    Unitofmeasurement = x.Unitofmeasurement,
                    FieldName = x.FieldName,
                    DefaultValue = x.DefaultValue,
                    DeviceType = x.DeviceType,
                    DataType = x.DataType


                });


                var serverattributes = serversRepository.Find(attributeexpression).AsQueryable().OrderBy(x => x.Id).FirstOrDefault();
               
                var serverValues = serverattributes.ToBsonDocument();
                foreach (var attri in attributes)
                {
                    if (serverValues.Contains(attri.FieldName))
                    {

                        var servervalue = serverValues.Where(x => x.Name == attri.FieldName).Select(x => x.Value).FirstOrDefault();
                        attri.DefaultValue = servervalue.ToString();
                        if (attri.DataType == "bool" && (attri.DefaultValue == "false" || attri.DefaultValue == "0"))
                        {
                            attri.DefaultboolValues = false;
                        }
                        else
                        {
                            attri.DefaultboolValues = true;
                        }

                        if (attri.FieldName == "password")
                        {
                            servervalue = serverValues.Where(x => x.Name == attri.FieldName).Select(x => x.Value).FirstOrDefault();

                            string myPassword = servervalue.ToString();
                            if (myPassword != "")
                            {
                                string[] myPasswordArray = myPassword.Split(',');
                                byte[] Password = new byte[myPasswordArray.Length];


                                for (int j = 0; j < myPasswordArray.Length; j++)
                                {
                                    Password[j] = Byte.Parse(myPasswordArray[j]);
                                }
                                servervalue = tripleDes.Decrypt(Password);

                                attri.DefaultValue = servervalue.ToString();

                            }
                        }

                    }
                    else
                    {
                        attri.DefaultValue = attri.DefaultValue;
                        if (attri.DataType == "bool" && (attri.DefaultValue == "false" || attri.DefaultValue == "0"))
                        {
                            attri.DefaultboolValues = false;
                        }
                        else
                        {
                            attri.DefaultboolValues = true;
                        }
                    }
                    serverresult.DeviceAttributes.Add(attri);
                }
                List<object> exchangeServers = null;
                if (serverresult.Devicetype == Enums.ServerType.DatabaseAvailabilityGroup.ToDescription())
                {
                    exchangeServers = serversRepository.Find(x => x.DeviceType == Enums.ServerType.Exchange.ToDescription()).ToList()
                        .Select(x => new { _id = x.Id, device_name = x.DeviceName }).Cast<object>().ToList();
                }
                //Response = Common.CreateResponse(serverresult);
                Response = Common.CreateResponse(new { credentialsData = credentialsData, serverresult = serverresult,
                    exchangeservers = exchangeServers != null ? exchangeServers : null });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching server atributes has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///Get Sametime Websphere data
        /// </summary>
        /// <author>Swathi</author>
       
        [HttpGet("get_sametime_websphere/{id}")]
        public APIResponse GetWebSphereSametimeData(string id)
        {
            try
            {

                WebShpereServerImport model = new WebShpereServerImport();
                // var cellsData = new List<CellInfo>();
                CellInfo cell = new CellInfo();
                serversRepository = new Repository<Server>(ConnectionString);
                //  var servers = serversRepository.Collection.AsQueryable().FirstOrDefault(x => x.SametimeId == id);
                // CellInfo cell = new CellInfo();
                List<NodeInfo> NodesData = new List<NodeInfo>();

                var websphereserver = serversRepository.All().Where(x => x.SametimeId == id && x.DeviceType == "WebSphereCell").FirstOrDefault();
                if (websphereserver != null)
                {
                    //  cellsData.Add(websphereserver);

                    cell.DeviceId = websphereserver.CellId;
                    cell.CellId = websphereserver.Id;
                    cell.CellName = websphereserver.CellName;
                    cell.Name = websphereserver.DeviceName;
                    cell.HostName = websphereserver.CellHostName;
                    cell.PortNo = websphereserver.PortNumber;
                    cell.ConnectionType = websphereserver.ConnectionType;
                    cell.GlobalSecurity = websphereserver.GlobalSecurity;
                    cell.CredentialsId = websphereserver.CredentialsId;
                    cell.Realm = websphereserver.Realm;

                    cell.NodesData = new List<NodeInfo>();
                    if (cell.NodesData != null)
                    {
                        foreach (var webSphereNode in websphereserver.Nodes)
                        {
                            foreach (var webSphereServer in webSphereNode.WebSphereServers)
                            {
                                if (serversRepository.Collection.AsQueryable().Where(x => x.Id == webSphereServer.ServerId).Count() == 0)
                                {
                                    NodeInfo node = new NodeInfo();
                                    node.NodeId = webSphereNode.NodeId;
                                    node.NodeName = webSphereNode.NodeName;
                                    node.ServerId = webSphereServer.ServerId;
                                    node.ServerName = webSphereServer.ServerName;
                                    node.HostName = webSphereNode.HostName;
                                    node.CellId = cell.CellId;
                                    cell.NodesData.Add(node);
                                }
                            }
                        }
                    }





                }

                //deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
                //model.DeviceAttributes = deviceAttributesRepository.All().Where(x => (x.DeviceType == Enums.ServerType.WebSphere.ToDescription())).Select(x => new DeviceAttributesModel
                //{
                //    Id = x.Id,
                //    AttributeName = x.AttributeName,
                //    DefaultValue = x.DefaultValue,
                //    DeviceType = x.DeviceType,
                //    FieldName = x.FieldName,
                //    Category = x.Category,
                //    DataType = x.DataType,
                //    Type = x.Type,
                //    Unitofmeasurement = x.Unitofmeasurement,
                //    IsSelected = false
                //}).OrderBy(x => x.AttributeName).ToList();

                credentialsRepository = new Repository<Credentials>(ConnectionString);

                var credentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();

                var credential = credentialsData.FirstOrDefault(x => x.Value == cell.CredentialsId);
                if (credential != null)
                    cell.CredentialsName = credential.DisplayText;

                model.SelectedServers = new List<NodeInfo>();
                Response = Common.CreateResponse(new { websphereData = model, cellData = cell, credentialsData = credentialsData });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving server attributes has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// //Get nodes
        /// <param name="cellInfo"></param>
        /// <param name="id"></param>
        /// <author>Swathi</author>
        /// <returns></returns>
        [HttpPut("get_sametime_websphere_nodes/{id}")]
        public APIResponse LoadSametimeWebSphereNodes(string id, [FromBody]CellInfo cellInfo)
        {
            FilterDefinition<NameValue> filterdef;
            string AppClientPath = "";
            string ServicePath = "";
            byte[] password;
            string decryptedPassword = string.Empty;
            string errorMessage = string.Empty;

            try
            {                
                serversRepository = new Repository<Server>(ConnectionString);
                //Get user name and password from credentials
                try
                {
                    credentialsRepository = new Repository<Credentials>(ConnectionString);
                    var credential = credentialsRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == cellInfo.CredentialsId);
                    if (!string.IsNullOrEmpty(credential.Password))
                    {
                        var passwordArray = credential.Password.Split(',');
                        password = new byte[passwordArray.Length];
                        for (int i = 0; i < passwordArray.Length; i++)
                        {
                            password[i] = Byte.Parse(passwordArray[i]);
                        }
                        VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                        cellInfo.Password = mySecrets.Decrypt(password);
                        cellInfo.UserName = credential.UserId;
                    }
                    else
                    {
                        cellInfo.Password = "";
                        cellInfo.UserName = "";
                    }

                    VitalSignsWebSphereDLL.VitalSignsWebSphereDLL dll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
                    VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellprop = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();
                    cellprop.HostName = cellInfo.HostName;
                    cellprop.Port = Convert.ToInt32(cellInfo.PortNo == null ? 0 : cellInfo.PortNo);
                    cellprop.ConnectionType = cellInfo.ConnectionType;
                    cellprop.UserName = cellInfo.UserName;
                    cellprop.Password = cellInfo.Password;
                    cellprop.Realm = cellInfo.Realm;

                    nameValueRepository = new Repository<NameValue>(ConnectionString);
                    filterdef = Builders<NameValue>.Filter.Where(p => p.Name == "WebSphereAppClientPath");
                    var apppath = nameValueRepository.Find(filterdef).Select(x => x.Value).FirstOrDefault();
                    if (apppath != null)
                    {
                        AppClientPath = apppath.ToString();
                    }
                    filterdef = Builders<NameValue>.Filter.Where(p => p.Name == "InstallLocation");
                    var servicepath = nameValueRepository.Find(filterdef).Select(x => x.Value).FirstOrDefault();
                    if (servicepath != null)
                    {
                        ServicePath = servicepath.ToString();
                    }
                    var cells = dll.getServerList(cellprop, AppClientPath, ServicePath);
                    //Line below used for testing only
                    //var cells = dll.getSrvList();
                    foreach (var cell in cells.Cell)
                    {
                        List<WebSphereNode> nodes = new List<WebSphereNode>();
                        Server server = serversRepository.Get(cellInfo.DeviceId);
                        if (string.IsNullOrEmpty(cellInfo.DeviceId))
                        {
                            Server sametimeserver = new Server();
                            sametimeserver.CellId = ObjectId.GenerateNewId().ToString();
                            sametimeserver.CellName = cellInfo.CellName;
                            sametimeserver.DeviceName = cellInfo.Name;
                            sametimeserver.CellHostName = cellInfo.HostName;
                            sametimeserver.ConnectionType = cellInfo.ConnectionType;
                            sametimeserver.PortNumber = cellInfo.PortNo;
                            sametimeserver.GlobalSecurity = cellInfo.GlobalSecurity;
                            sametimeserver.CredentialsId = cellInfo.CredentialsId;
                            sametimeserver.Realm = cellInfo.Realm;
                            sametimeserver.SametimeId = id;
                            sametimeserver.DeviceType = Enums.ServerType.WebSphereCell.ToDescription();
                            var serverId = serversRepository.Insert(sametimeserver);
                            Response = Common.CreateResponse(serverId, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell inserted successfully");
                        }
                        else
                        {
                            FilterDefinition<Server> sametimefilterDefination = Builders<Server>.Filter.Where(p => p.Id == cellInfo.DeviceId);
                            var updateSametimeDefination = serversRepository.Updater.Set(p => p.CellId, cellInfo.CellId)
                                .Set(p => p.CellName, cellInfo.CellName)
                                .Set(p => p.DeviceName, cellInfo.Name)
                                .Set(p => p.CellHostName, cellInfo.HostName)
                                .Set(p => p.ConnectionType, cellInfo.ConnectionType)
                                .Set(p => p.PortNumber, cellInfo.PortNo)
                                .Set(p => p.GlobalSecurity, cellInfo.GlobalSecurity)
                                .Set(p => p.CredentialsId, cellInfo.CredentialsId)
                                .Set(p => p.Realm, cellInfo.Realm);
                            var result = serversRepository.Update(sametimefilterDefination, updateSametimeDefination);
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell updated successfully");
                        }
                        foreach (var cellNode in cell.Nodes.Node)
                        {
                            WebSphereNode node = new WebSphereNode();
                            node.NodeId = ObjectId.GenerateNewId().ToString();
                            node.NodeName = cellNode.Name;
                            node.HostName = cellNode.HostName;
                            node.WebSphereServers = new List<WebSphereServer>();
                            foreach (var nodeServer in cellNode.Servers.Server)
                            {
                                WebSphereServer webSphereServer = new WebSphereServer();
                                webSphereServer.ServerId = ObjectId.GenerateNewId().ToString();
                                webSphereServer.ServerName = nodeServer;
                                node.WebSphereServers.Add(webSphereServer);

                            }
                            nodes.Add(node);
                        }
                        var deviceId = serversRepository.Collection.AsQueryable().FirstOrDefault(x => x.SametimeId == id);
                        FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == deviceId.Id);
                        var updateDefination = serversRepository.Updater.Set(p => p.CellName, cell.Name).Set(p => p.Nodes, nodes);
                        var noderesult = serversRepository.Update(filterDefination, updateDefination);
                        //Call get_advanced_settings to get the current node info
                        Response = GetAdvancedSettings(id);
                        //Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell updated successfully");
                    }
                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, "Error", "Refreshing WebSphere information has failed.\n Error Message :" + exception.Message);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Refreshing WebSphere information has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        ///saves the Server device attributes data
        /// </summary>
        /// <author>Swathi</author>
        [HttpPut("save_servers_attributes/{id}")]
        public APIResponse SaveServerDeviceAttributes([FromBody]DeviceSettings serverAttributes, string id)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                var deviceAttributes = ((Newtonsoft.Json.Linq.JObject)serverAttributes.Value).ToObject<DeviceAttributesDataModel>();
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                if (!string.IsNullOrEmpty(deviceAttributes.LocationId))
                {
                    var locationname = locationRepository.All().Where(x => x.LocationName == deviceAttributes.LocationId).Select(x => new Location
                    {
                        Id = x.Id


                    }).FirstOrDefault();
                    deviceAttributes.LocationId = locationname.Id;
                }
                else
                {
                    deviceAttributes.LocationId = null;
                }
                //   UpdateDefinition<BsonDocument> updateserverDefinition = Builders<BsonDocument>.Update.Set(devicename=devicename,, category, ipaddress, isenabled, location, description);
                //  var serverresult = repository.Collection.UpdateMany(filter, updateserverDefinition);
                ObjectId objectIdTest = new ObjectId();
                if (!ObjectId.TryParse(deviceAttributes.CredentialsId, out objectIdTest))
                {
                    deviceAttributes.CredentialsId = null;
                }
                var updateDefination = serversRepository.Updater.Set(p => p.DeviceName, deviceAttributes.DeviceName)
                    .Set(p => p.Category, deviceAttributes.Category)
                    .Set(p => p.HostName, deviceAttributes.HostName)
                    .Set(p => p.PortName, deviceAttributes.PortName)
                    .Set(p => p.IPAddress, deviceAttributes.IPAddress)
                    .Set(p => p.LocationId, deviceAttributes.LocationId)
                    .Set(p => p.Description, deviceAttributes.Description)
                    .Set(p => p.IsEnabled, deviceAttributes.IsEnabled)
                    .Set(p => p.RequireSSL, deviceAttributes.RequireSSL)
                    .Set(p => p.Platform, deviceAttributes.Platform)
                    .Set(p => p.Mode, deviceAttributes.Mode)
                    .Set(p => p.CredentialsId, deviceAttributes.CredentialsId)
                    .Set(p => p.AuthenticationType, deviceAttributes.AuthenticationType);

                var serverresult = serversRepository.Update(filterDefination, updateDefination);

                if (deviceAttributes.DeviceAttributes.Count() > 0)
                {
                    foreach (var attribute in deviceAttributes.DeviceAttributes)
                    {
                        if (!string.IsNullOrEmpty(attribute.FieldName))
                        {
                            if (attribute.FieldName != "is_enabled" && attribute.FieldName != "require_ssl")
                            {
                                string field = attribute.FieldName;
                                string value = attribute.DefaultValue;
                                bool defaultvalues = attribute.DefaultboolValues;
                                string datatype = attribute.DataType;
                                if (field == "password")
                                {

                                    byte[] myPassWord;

                                    VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                                    myPassWord = mySecrets.Encrypt(attribute.DefaultValue);

                                    System.Text.StringBuilder newString = new System.Text.StringBuilder();
                                    foreach (byte b in myPassWord)
                                    {
                                        newString.AppendFormat("{0}, ", b);
                                    }
                                    string bytePassword = newString.ToString();
                                    int n = bytePassword.LastIndexOf(", ");
                                    bytePassword = bytePassword.Substring(0, n);
                                    value = bytePassword;

                                }
                                if (datatype == "int")
                                {
                                    int outputvalue = Convert.ToInt32(value);
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                         .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "double")
                                {
                                    double outputvalue = Convert.ToDouble(value);
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                         .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "bool")
                                {
                                    //3/6/2017 NS modified for VSPLUS-3521
                                    bool booloutput = (attribute.DefaultboolValues == false ? false : true);
                                    //string booloutput;
                                    //if (defaultvalues == false)
                                    //{
                                    //    booloutput = "false";
                                    //}
                                    //else
                                    //{
                                    //    booloutput = "true";
                                    //}
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, booloutput);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "string")
                                {
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, value);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "ObjectId")
                                {
                                    if (value != "" && value != null)
                                    {
                                        UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, ObjectId.Parse(value));
                                        var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                    }
                                }
                            }   
                        }
                    }
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server attributes updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving server attributes has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }


        #region Disk Settings
        /// <summary>
        ///Returns Disk Information
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_server_disk_info/{id}")]
        public APIResponse GetServerDiskInformation(string id)

        {

            try
            {
                statusRepository = new Repository<Status>(ConnectionString);
                Expression<Func<Status, bool>> statusExpression = (p => p.DeviceId == id);
                var statusResult = statusRepository.Find(statusExpression).Select(x => x.Disks).FirstOrDefault();


                SelectedDiksModel serverDiskStatus = new SelectedDiksModel();
                List<SelectedDiksModel> drives = new List<SelectedDiksModel>();
                foreach (DiskStatus drive in statusResult)
                {
                    drives.Add(new SelectedDiksModel
                    {
                        DiskFree = drive.DiskFree,

                        DiskName = drive.DiskName,
                        DiskSize = drive.DiskSize,

                        PercentFree = drive.PercentFree.HasValue ? drive.PercentFree.Value * 100 : drive.PercentFree.Value,

                    });

                }
                serversRepository = new Repository<Server>(ConnectionString);

                var serversDiskInfo = serversRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == id);
                if (serversDiskInfo.DiskInfo != null)
                {
                    foreach (var item in serversDiskInfo.DiskInfo)
                    {
                        if (drives.FirstOrDefault(x => x.DiskName == item.DiskName) != null)
                        {
                            drives.FirstOrDefault(x => x.DiskName == item.DiskName).FreespaceThreshold = item.Threshold.ToString();

                            drives.FirstOrDefault(x => x.DiskName == item.DiskName).ThresholdType = item.ThresholdType.ToString();
                            if (drives.FirstOrDefault(x => x.DiskName == item.DiskName).ThresholdType == "Percent")
                                drives.FirstOrDefault(x => x.DiskName == item.DiskName).FreespaceThreshold = (Convert.ToDouble(drives.FirstOrDefault(x => x.DiskName == item.DiskName).FreespaceThreshold) * 100).ToString();
                            drives.FirstOrDefault(x => x.DiskName == item.DiskName).IsSelected = true;

                        }
                    }
                }
                Response = Common.CreateResponse(drives);
            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error getting disk information");
            }



            return Response;
        }
        /// <summary>
        ///Getting Disk Data
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_server_disk_settings_data/{id}")]
        public APIResponse GetServerDiskSettingsData(string id)

        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                var result = serversRepository.Find(expression).Select(x => x.DiskInfo).FirstOrDefault();
                if (result.Count == 1)
                {
                    var results = result.Select(s => new SelectedDiksModel
                    {
                        DiskName = (s.DiskName == "AllDisks" && s.ThresholdType == "GB" ? "allDisksByGB" :
                                                                         s.DiskName == "AllDisks" && s.ThresholdType == "Percent" ? "allDisksBypercentage" :
                                                                         s.DiskName == "NoAlerts" ? "noDiskAlerts" :
                                                                         "selectedDisks"),
                        ThresholdType = s.ThresholdType,
                        FreespaceThreshold = s.Threshold.ToString()

                    }).FirstOrDefault();
                    Response = Common.CreateResponse(results);
                }
                else
                {
                    Response = Common.CreateResponse(new SelectedDiksModel
                    {
                        DiskName = "selectedDisks",
                        ThresholdType = "GB",
                        FreespaceThreshold = "0"
                    });
                }
                
            }



            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting server disk settings data has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// Updates Disk Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_server_disk_settings")]
        public APIResponse SaveServerDiskSettings([FromBody]DeviceSettings deviceSettings)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {

                string setting = Convert.ToString(deviceSettings.Setting);
                string settingValue = Convert.ToString(deviceSettings.Value);

                UpdateDefinition<Server> updateDefinition = null;
                if (!string.IsNullOrEmpty(setting))
                {


                    var server = serversRepository.Get(deviceSettings.Devices.ToString());
                    List<DiskSetting> diskSettings = new List<DiskSetting>();

                    if (setting.Equals("allDisksBypercentage"))
                        diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "Percent" });
                    else if (setting.Equals("allDisksByGB"))
                        diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "GB" });
                    else if (setting.Equals("noDiskAlerts"))
                        diskSettings.Add(new DiskSetting { DiskName = "NoAlerts", Threshold = null, ThresholdType = null });
                    else if (setting.Equals("selectedDisks"))
                    {
                        List<SelectedDiksModel> selectedDisks = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Value).ToObject<List<SelectedDiksModel>>();
                        foreach (var item in selectedDisks)
                        {
                            if (item.IsSelected)
                            {
                                diskSettings.Add(new DiskSetting {
                                    DiskName = item.DiskName,
                                    Threshold = item.ThresholdType == "Percent" ? Convert.ToDouble(item.FreespaceThreshold) / 100 : Convert.ToDouble(item.FreespaceThreshold),
                                    ThresholdType = item.ThresholdType
                                });
                            }
                        }
                    }
                    if (diskSettings.Count > 0)
                    {
                        updateDefinition = serversRepository.Updater.Set(p => p.DiskInfo, diskSettings);
                        var result = serversRepository.Update(server, updateDefinition);
                    }

                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Server disk settings updated successfully");

                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "No servers were selected");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving server disk settings has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Server Tasks


        /// <summary>
        ///get the server tasks data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpGet("get_server_tasks_info/{id}")]
        public APIResponse GetServerTasksInformation(string id)

        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                var result = serversRepository.Find(expression).Select(x => x.ServerTasks).FirstOrDefault();
                List<DominoServerTasksModel> servertasks = new List<DominoServerTasksModel>();
                foreach (DominoServerTask task in result)
                {
                    servertasks.Add(new DominoServerTasksModel
                    {
                        Id = task.Id,
                        TaskId = task.TaskId,
                        IsLoad = task.SendLoadCmd,
                        IsResartLater = task.SendRestartCmd,
                        IsRestartASAP = task.SendRestartCmdOffhours,
                        IsDisallow = task.SendExitCmd,
                        TaskName = task.TaskName,
                        IsSelected = task.Monitored,
                        DeviceId = id

                    });

                }

                Response = Common.CreateResponse(servertasks);
            }


            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, "Error", "Error getting server tasks");
            }



            return Response;
        }



        /// <summary>
        ///Get tasks names data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpGet("get_tasks_names")]
        public APIResponse GetTaskNames(string id)

        {

            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                var result1 = dominoservertasksRepository.All().Where(x => x.TaskName != null).Select(x => x.TaskName).Distinct().OrderBy(x => x).ToList();
                Response = Common.CreateResponse(new { TaskNames = result1 });

            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error getting task names");
            }



            return Response;
        }

        /// <summary>
        ///saves the server tasks data
        /// </summary>
        /// <author>Sowjanya</author>

        [HttpPut("save_server_tasks")]
        public APIResponse SaveServerTasksData([FromBody]DominoServerTasksModel servertasks)
        {

            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                Expression<Func<DominoServerTasks, bool>> filterExpression = (p => p.TaskName == servertasks.TaskName);
                var taskId = dominoservertasksRepository.Find(filterExpression).Select(x => x.Id).FirstOrDefault();

                Expression<Func<Server, bool>> filterExpression1 = (p => p.Id == servertasks.DeviceId);
                var existsData = serversRepository.Find(filterExpression1).Select(x => x.ServerTasks).ToList();
                foreach (var data in existsData)
                {
                    foreach (var nameData in data)
                    {
                        if (nameData.Id != servertasks.Id)
                        {
                            if (nameData.TaskName == servertasks.TaskName)
                            {
                                name = "exists";
                            }
                        }


                    }
                }
                if (name == "exists")
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This task name already exists. Please enter a different one.");
                }
                else
                {
                    List<DominoServerTask> serverTasks = new List<DominoServerTask>();
                    var server = serversRepository.Collection.AsQueryable().FirstOrDefault(p => p.Id == servertasks.DeviceId);


                    if (string.IsNullOrEmpty(servertasks.Id))
                    {
                        DominoServerTask dominoServerTask = new DominoServerTask();
                        dominoServerTask.TaskId = taskId;
                        dominoServerTask.TaskName = servertasks.TaskName;
                        dominoServerTask.SendLoadCmd = servertasks.IsLoad;
                        dominoServerTask.Monitored = servertasks.IsSelected;
                        dominoServerTask.SendRestartCmd = servertasks.IsResartLater;
                        dominoServerTask.SendRestartCmdOffhours = servertasks.IsRestartASAP;
                        dominoServerTask.SendExitCmd = servertasks.IsDisallow;
                        dominoServerTask.Id = ObjectId.GenerateNewId().ToString();
                        serverTasks = server.ServerTasks;
                        serverTasks.Add(dominoServerTask);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Domino server tasks inserted successfully");

                    }
                    else
                    {
                        foreach (var serverTask in server.ServerTasks)
                        {
                            if (serverTask.Id.Equals(servertasks.Id))
                            {
                                serverTask.TaskId = taskId;
                                serverTask.TaskName = servertasks.TaskName;
                                serverTask.SendLoadCmd = servertasks.IsLoad;
                                serverTask.Monitored = servertasks.IsSelected;
                                serverTask.SendRestartCmd = servertasks.IsResartLater;
                                serverTask.SendRestartCmdOffhours = servertasks.IsRestartASAP;
                                serverTask.SendExitCmd = servertasks.IsDisallow;

                            }
                        }
                        serverTasks = server.ServerTasks;
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Domino server tasks updated successfully");
                    }
                    var updateDefinitaion = serversRepository.Updater.Set(p => p.ServerTasks, serverTasks);
                    var filterDefination = Builders<Server>.Filter.Where(p => p.Id == servertasks.DeviceId);
                    serversRepository.Update(filterDefination, updateDefinitaion);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving Domino server tasks has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        /// <summary>
        ///delete the  server tasks data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpDelete("delete_server_tasks/{deviceId}/{id}")]
        public APIResponse DeleteServerTasks(string deviceId, string id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {

                var server = serversRepository.Get(deviceId);
                var dominoServerTasks = server.ServerTasks;

                var serverTaskDelete = dominoServerTasks.FirstOrDefault(x => x.Id == id);
                if (serverTaskDelete != null)
                {
                    dominoServerTasks.Remove(serverTaskDelete);
                    var updateDefinition = serversRepository.Updater.Set(p => p.ServerTasks, dominoServerTasks);
                    var result = serversRepository.Update(server, updateDefinition);
                }
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Server tasks deleted successfully");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of server tasks has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        #endregion

        #region Advanced Settings
        /// <summary>
        /// Get Advanced  Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_advanced_settings/{id}")]
        public APIResponse GetAdvancedSettings(string id)
        {
            List<NodeInfo> NodesData = new List<NodeInfo>();
            try
            {
                CellInfo cell = new CellInfo();
                cell.NodesData = new List<NodeInfo>();
                serversRepository = new Repository<Server>(ConnectionString);
        
                var websphereserver = serversRepository.All().Where(x => x.SametimeId == id && x.DeviceType == "WebSphereCell").FirstOrDefault();
                if (websphereserver != null)
                {
                    cell.DeviceId = websphereserver.CellId;
                    cell.CellId = websphereserver.Id;
                    cell.CellName = websphereserver.CellName;
                    cell.Name = websphereserver.DeviceName;
                    cell.HostName = websphereserver.CellHostName;
                    cell.PortNo = websphereserver.PortNumber;
                    cell.ConnectionType = websphereserver.ConnectionType;
                    cell.GlobalSecurity = websphereserver.GlobalSecurity;
                    cell.CredentialsId = websphereserver.CredentialsId;
                    cell.Realm = websphereserver.Realm;
                    foreach (var webSphereNode in websphereserver.Nodes)
                    {
                        foreach (var webSphereServer in webSphereNode.WebSphereServers)
                        {
                            if (serversRepository.Collection.AsQueryable().Where(x => x.Id == webSphereServer.ServerId).Count() == 0)
                            {
                                NodeInfo node = new NodeInfo();
                                node.NodeId = webSphereNode.NodeId;
                                node.NodeName = webSphereNode.NodeName;
                                node.ServerId = webSphereServer.ServerId;
                                node.ServerName = webSphereServer.ServerName;
                                node.HostName = webSphereNode.HostName;
                                node.CellId = cell.CellId;
                                cell.NodesData.Add(node);
                            }
                        }
                    }
                }
                else
                {
                    cell.DeviceId = "";
                    cell.CellId = "";
                    cell.CellName = "";
                    cell.Name = "";
                    cell.HostName = "";
                    cell.PortNo = 0;
                    cell.ConnectionType = "";
                    cell.GlobalSecurity = false;
                    cell.CredentialsId = "";
                    cell.Realm = "";
                    cell.NodesData = new List<NodeInfo>();
                }
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                var wsCredentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
                var credential = wsCredentialsData.FirstOrDefault(x => x.Value == cell.CredentialsId);
                if (credential != null)
                {
                    cell.CredentialsName = credential.DisplayText;
                }
                var platform = serversRepository.Collection.AsQueryable().Where(x => x.Id == id).Select(x => x.Platform).FirstOrDefault();
                var dbResults = serversRepository.Collection.AsQueryable().Where(x => x.Id == id).ToList();
                var results = dbResults
                            .Select(x => new AdvancedSettingsModel
                            {
                                MemoryThreshold = x.MemoryThreshold == null ? 0 : x.MemoryThreshold * 100,
                                CpuThreshold = x.CpuThreshold == null ? 0 : x.CpuThreshold * 100,
                                ServerDaysAlert = x.ServerDaysAlert,
                                ClusterReplicationDelayThreshold = x.ClusterReplicationDelayThreshold,
                                ProxyServerType = x.ProxyServerType,
                                ProxyServerprotocol = x.ProxyServerprotocol,
                                DbmsHostName = x.DbmsHostName,
                                DbmsName = x.DbmsName,
                                DbmsPort = x.DbmsPort,
                                Db2SettingsCredentialsId = x.Db2SettingsCredentialsId,
                                CollectExtendedStatistics = x.CollectExtendedStatistics,
                                CollectMeetingStatistics = x.CollectExtendedStatistics,
                                ExtendedStatisticsPort = x.ExtendedStatisticsPort,
                                MeetingHostName = x.MeetingHostName,
                                MeetingPort = x.MeetingPort,
                                MeetingRequireSSL = x.MeetingRequireSSL,
                                ConferenceHostName = x.ConferenceHostName,
                                ConferencePort = x.ConferencePort,
                                ConferenceRequireSSL = x.ConferenceRequireSSL,
                                DatabaseSettingsHostName = x.DatabaseSettingsHostName,
                                DatabaseSettingsCredentialsId = x.DatabaseSettingsCredentialsId,
                                DatabaseSettingsPort = x.DatabaseSettingsPort,
                                DeviceType = x.DeviceType,
                                DominoServerName = x.DominoServerName,
                                CollectConferenceStatistics = x.CollectConferenceStatistics,
                                ClusterReplicationQueueThreshold = x.ClusterReplicationQueueThreshold,
                                SimulationTests = x.SimulationTests != null ? x.SimulationTests.Select(y => new NameValueModel() { Name = y.Name, Value = y.Value}).ToList(): null,
                                CasCredentialsId = x.ActiveSyncCredentialsId,
                                
                            }).FirstOrDefault();
                //var simulationTests = dbResults.Select(x => x.SimulationTests).FirstOrDefault();
                if (results != null)
                {
                    results.DeviceId = cell.DeviceId;
                    results.CellId = cell.CellId;
                    results.CellName = cell.CellName;
                    results.Name = cell.Name;
                    results.HostName = cell.HostName;
                    results.PortNo = cell.PortNo;
                    results.ConnectionType = cell.ConnectionType;
                    results.GlobalSecurity = cell.GlobalSecurity;
                   results.CredentialsId = cell.CredentialsId;
                    results.Realm = cell.Realm;
                    results.NodesData = cell.NodesData;
                }
                var credentialsData = credentialsRepository.Collection.AsQueryable().Where(x => x.DeviceType == results.DeviceType).Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);                          
                Response = Common.CreateResponse(new { results = results, platform = platform, credentialsData = credentialsData, wsCredentialsData = wsCredentialsData});
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error getting advanced settings. " + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// Updates Advanced  Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_advanced_settings/{id}")]
        public APIResponse UpdateAdvancedSettings([FromBody]AdvancedSettingsModel advancedSettings, string id)
        {
            try
            {
                FilterDefinition<Server> filterDefinition = Builders<Server>.Filter.Where(p => p.Id == id);
                serversRepository = new Repository<Server>(ConnectionString);
                try
                {
                    if (advancedSettings.DeviceType == "Domino")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.MemoryThreshold, advancedSettings.MemoryThreshold/100)
                            .Set(p => p.CpuThreshold, advancedSettings.CpuThreshold/100)
                            .Set(p => p.ServerDaysAlert, advancedSettings.ServerDaysAlert)
                            .Set(p => p.ClusterReplicationDelayThreshold, advancedSettings.ClusterReplicationDelayThreshold)
                            .Set(p => p.ClusterReplicationQueueThreshold, advancedSettings.ClusterReplicationQueueThreshold);
                        var result = serversRepository.Update(filterDefinition, updateDefination);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    }
                    else if (advancedSettings.DeviceType == "Sametime")
                    {
                        var updateDefinition = serversRepository.Updater.Set(p => p.ProxyServerType, advancedSettings.ProxyServerType)
                            .Set(p => p.ProxyServerprotocol, advancedSettings.ProxyServerprotocol)
                            .Set(p => p.DbmsHostName, advancedSettings.DbmsHostName)
                            .Set(p => p.DbmsName, advancedSettings.DbmsName)
                            .Set(p => p.DbmsPort, advancedSettings.DbmsPort)
                            .Set(p => p.CollectExtendedStatistics, advancedSettings.CollectExtendedStatistics)
                            .Set(p => p.CollectMeetingStatistics, advancedSettings.CollectMeetingStatistics)
                            .Set(p => p.ExtendedStatisticsPort, advancedSettings.ExtendedStatisticsPort)
                            .Set(p => p.DbmsHostName, advancedSettings.DbmsHostName)
                            .Set(p => p.MeetingHostName, advancedSettings.MeetingHostName)
                            .Set(p => p.MeetingPort, advancedSettings.MeetingPort)
                            .Set(p => p.MeetingRequireSSL, advancedSettings.MeetingRequireSSL)
                            .Set(p => p.ConferenceHostName, advancedSettings.ConferenceHostName)
                            .Set(p => p.ConferencePort, advancedSettings.ConferencePort)
                            .Set(p => p.ConferenceRequireSSL, advancedSettings.ConferenceRequireSSL)
                            .Set(p => p.CollectConferenceStatistics, advancedSettings.CollectConferenceStatistics)
                            .Set(p => p.DominoServerName, advancedSettings.DominoServerName)
                            .Set(p => p.Db2SettingsCredentialsId, advancedSettings.Db2SettingsCredentialsId);

                        var result = serversRepository.Update(filterDefinition, updateDefinition, new UpdateOptions { IsUpsert = true });
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    }
                    else if (advancedSettings.DeviceType == "IBM Connections")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.DatabaseSettingsHostName, advancedSettings.DatabaseSettingsHostName)
                            .Set(p => p.DatabaseSettingsPort, advancedSettings.DatabaseSettingsPort)
                            .Set(p => p.DatabaseSettingsCredentialsId, advancedSettings.DatabaseSettingsCredentialsId);
                        var result = serversRepository.Update(filterDefinition, updateDefination);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    } else if (advancedSettings.DeviceType == "Exchange")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.MemoryThreshold, advancedSettings.MemoryThreshold / 100)
                            .Set(p => p.CpuThreshold, advancedSettings.CpuThreshold / 100)
                            .Set(p => p.ServerDaysAlert, advancedSettings.ServerDaysAlert)
                            .Set(p => p.SimulationTests, advancedSettings.SimulationTests.Where(x => x.Value != "False").Select(x => new NameValuePair() { Name = x.Name }).ToList())
                            .Set(p => p.ActiveSyncCredentialsId, advancedSettings.CasCredentialsId);
                        var result = serversRepository.Update(filterDefinition, updateDefination);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    }
                    else if (advancedSettings.DeviceType == "SharePoint")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.MemoryThreshold, advancedSettings.MemoryThreshold / 100)
                            .Set(p => p.CpuThreshold, advancedSettings.CpuThreshold / 100)
                            .Set(p => p.ServerDaysAlert, advancedSettings.ServerDaysAlert)
                            .Set(p => p.SimulationTests, advancedSettings.SimulationTests.Where(x => x.Value != "False").Select(x => new NameValuePair() { Name = x.Name }).ToList());
                        var result = serversRepository.Update(filterDefinition, updateDefination);
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    }
                    else if (advancedSettings.DeviceType == "IBM FileNet")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.MemoryThreshold, advancedSettings.MemoryThreshold / 100)
                            .Set(p => p.CpuThreshold, advancedSettings.CpuThreshold / 100)
                            .Set(p => p.ServerDaysAlert, advancedSettings.ServerDaysAlert);
                        var result = serversRepository.Update(filterDefinition, updateDefination);
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advanced settings updated successfully");
                    }
                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving advanced settings has failed.\n Error Message :" + exception.Message);
                }

                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving advanced settings has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region dag database settings

        [HttpGet("get_dag_database_settings_data/{id}")]
        public APIResponse GetDagDatabaseSettingsData(string id)

        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                var result = serversRepository.Find(expression).Select(x => x.DatabaseInfo).FirstOrDefault();

                if (result.Count == 1)
                {
                    var results = result.Select(s => new SelectedDagDatabaseModel
                    {
                        DatabaseName = (s.DatabaseName == "allDatabases" ? "allDatabases" :
                                                                         s.DatabaseName == "noAlerts" ? "noAlerts" :
                                                                         "selectedDatabases"),
                        ReplayQueueThreshold = s.ReplayThreshold,
                        CopyQueueThreshold = s.CopyThreshold

                    }).FirstOrDefault();
                    Response = Common.CreateResponse(results);
                }






            }


            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting server disk settings data has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }


        [HttpGet("get_dag_database_info/{id}")]
        public APIResponse GetDagDatabaseSettingsInfo(string id)

        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                statusRepository = new Repository<Status>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                Expression<Func<Status, bool>> expressionStatus = (p => p.DeviceId == id);
                var status = statusRepository.Find(expressionStatus).ToList();
                var result = serversRepository.Find(expression).Select(x => x.DatabaseInfo).FirstOrDefault();
                if (result.Count == 1)
                {
                    var results = result.Select(s => new SelectedDagDatabaseModel
                    {
                        DatabaseName = s.DatabaseName,
                        ServerName = s.ServerName,
                        CopyQueueThreshold = s.CopyThreshold,
                        ReplayQueueThreshold = s.ReplayThreshold,
                        CurrentReplayQueue = status.Count() > 0 ? status[0].DagServerDatabases.Where(x => s.DatabaseName == x.DatabaseName && s.ServerName == x.ServerName).FirstOrDefault().ReplayQueue : 0,
                        CurrentCopyQueue = status.Count() > 0 ? status[0].DagServerDatabases.Where(x => s.DatabaseName == x.DatabaseName && s.ServerName == x.ServerName).FirstOrDefault().CopyQueue : 0,
                        IsSelected = status.Count() > 0 ? status[0].DagServerDatabases.Where(x => s.DatabaseName == x.DatabaseName && s.ServerName == x.ServerName).Count() > 0 : false
                    }).ToList();
                    Response = Common.CreateResponse(results);
                }
                
            }


            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting dag database settings data has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_dag_database_settings")]
        public APIResponse SaveDagDatabaseSettings([FromBody]List<SelectedDagDatabaseModel> deviceSettings, string id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                
                List<DagDatabases> dagDatabases = new List<DagDatabases>();

                foreach(SelectedDagDatabaseModel deviceSetting in deviceSettings.Where(x => x.IsSelected))
                {
                    dagDatabases.Add(new DagDatabases()
                    {
                        CopyThreshold = deviceSetting.CopyQueueThreshold,
                        ReplayThreshold = deviceSetting.ReplayQueueThreshold,
                        ServerName = deviceSetting.ServerName,
                        DatabaseName = deviceSetting.DatabaseName
                    });
                }

                serversRepository.Update(
                    serversRepository.Filter.Eq(x => x.Id, id),
                    serversRepository.Updater.Set(x => x.DatabaseInfo, dagDatabases)
                    );

                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Database settings updated successfully");

                /*
                if (!string.IsNullOrEmpty(setting))
                {


                    var server = serversRepository.Get(deviceSettings.Devices.ToString());
                    List<DiskSetting> diskSettings = new List<DiskSetting>();

                    if (setting.Equals("allDisksBypercentage"))
                        diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "Percent" });
                    else if (setting.Equals("allDisksByGB"))
                        diskSettings.Add(new DiskSetting { DiskName = "AllDisks", Threshold = Convert.ToDouble(settingValue), ThresholdType = "GB" });
                    else if (setting.Equals("noDiskAlerts"))
                        diskSettings.Add(new DiskSetting { DiskName = "NoAlerts", Threshold = null, ThresholdType = null });
                    else if (setting.Equals("selectedDisks"))
                    {
                        List<SelectedDiksModel> selectedDisks = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Value).ToObject<List<SelectedDiksModel>>();
                        foreach (var item in selectedDisks)
                        {
                            if (item.IsSelected)
                            {
                                diskSettings.Add(new DiskSetting { DiskName = item.DiskName, Threshold = Convert.ToDouble(item.FreespaceThreshold), ThresholdType = item.ThresholdType });
                            }
                        }
                    }
                    if (diskSettings.Count > 0)
                    {
                        updateDefinition = serversRepository.Updater.Set(p => p.DiskInfo, diskSettings);
                        var result = serversRepository.Update(server, updateDefinition);
                    }

                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Server disk settings updated successfully");
                    
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "No servers were selected");
                }
                */
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving server disk settings has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Office365 Settings

        [HttpGet("get_office365_nodes")]
        public APIResponse GetOffice365Nodes(string deviceId)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                nodesRepository = new Repository<Nodes>(ConnectionString);
                var server = serversRepository.Find(serversRepository.Filter.Eq(x => x.Id, deviceId)).ToList().First();
                var nodes = nodesRepository.Find(x => true).ToList();

                List<Office365Node> o365Nodes = new List<Office365Node>();
                foreach(var node in nodes)
                {
                    Office365Node o365Node = new Office365Node();
                    o365Node.NodeId = node.Id;
                    o365Node.HostName = node.HostName;
                    o365Node.Location = node.Location == "" ? node.Name : node.Location;
                    o365Node.IsSelected = (server.NodeIds != null && server.NodeIds.Contains(node.Id) == true);
                    o365Nodes.Add(o365Node);
                }
                
                
                Response = Common.CreateResponse(o365Nodes);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error getting Office 365 Nodes.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_office365_nodes")]
        public APIResponse SaveOffice365Nodes([FromBody]List<Office365Node> office365Nodes, string deviceId)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                FilterDefinition<Server> filterDef = serversRepository.Filter.Eq(x => x.Id, deviceId);
                UpdateDefinition<Server> updateDef = serversRepository.Updater.Set(x => x.NodeIds, office365Nodes.Where(x => x.IsSelected).Select(x => x.NodeId).ToList());
                serversRepository.Update(filterDef, updateDef);
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server attributes updated successfully");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error setting Office 365 Nodes.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion
        #endregion

        #region IBM Domino Settings

        #region Custom Statistics
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_custom_statistics")]
        public APIResponse GetCustomStatistics()
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type == "Domino Custom Statistic").Select(x => new CustomStatisticsModel
                {
                    Id = x.Id,
                    DominoServers = x.DominoServers,
                    StatName = x.StatName,
                    ThresholdValue = x.ThresholdValue,
                    TimesInARow = x.TimesInARow,
                    GreaterThanOrLessThan = x.GreaterThanOrLessThan,
                    ConsoleCommand = x.ConsoleCommand,
                    TypeOfStatistic=x.TypeOfStatistic,
                    EqualOrNotEqual=x.EqualOrNotEqual



                }).ToList();
                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting custom statistics has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="customstat"></param>
        /// <returns></returns>
        [HttpPut("save_custom_statistics")]
        public APIResponse UpdateCustomStatistics([FromBody]CustomStatisticsModel customstat)
        {
            try
            {
                var devicesList = customstat.DominoServers;
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                if (string.IsNullOrEmpty(customstat.Id))
                {
                    ServerOther customstatistic = new ServerOther
                    {
                        Type = "Domino Custom Statistic",
                        DominoServers = devicesList,
                        StatName = customstat.StatName,
                        ThresholdValue = customstat.ThresholdValue,
                        TimesInARow = customstat.TimesInARow,
                        GreaterThanOrLessThan = customstat.GreaterThanOrLessThan,
                        ConsoleCommand = customstat.ConsoleCommand,
                        TypeOfStatistic=customstat.TypeOfStatistic,
                        EqualOrNotEqual=customstat.EqualOrNotEqual

                    };
                    string id = serverOtherRepository.Insert(customstatistic);
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Custom statistics inserted successfully");
                }
                else
                {
                    FilterDefinition<ServerOther> filterDefination = Builders<ServerOther>.Filter.Where(p => p.Id == customstat.Id);
                    var updateDefination = serverOtherRepository.Updater.Set(p => p.DominoServers, devicesList)
                                                             .Set(p => p.StatName, customstat.StatName)
                                                             .Set(p => p.ThresholdValue, customstat.ThresholdValue)
                                                             .Set(p => p.TimesInARow, customstat.TimesInARow)
                                                              .Set(p => p.GreaterThanOrLessThan, customstat.GreaterThanOrLessThan)
                                                               .Set(p => p.ConsoleCommand, customstat.ConsoleCommand)
                                                               .Set(p => p.TypeOfStatistic, customstat.TypeOfStatistic)
                                                               .Set(p => p.EqualOrNotEqual, customstat.EqualOrNotEqual);
                    var result = serverOtherRepository.Update(filterDefination, updateDefination);
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Custom statistics updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving custom statistics has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("delete_custom_statistics/{Id}")]
        public APIResponse DeleteCustomStatistics(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Custom statistics deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of custom statistics has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion

        #region Notes Database Replicas
        /// <summary>
        /// Returns Domino Server Names
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_domino_servers")]
        public APIResponse GetDominoServerNames()
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var serversData = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType == "Domino").Select(x => new ComboBoxListItem { DisplayText = x.DeviceName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                Response = Common.CreateResponse(new { serversData = serversData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching Domino servers has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// Returns Notes Database Replica
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_notes_database_replica")]
        public APIResponse GetAllNotesDatabaseReplica()
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type == "Notes Database Replica").Select(x => new NotesDatabaseReplicaModel
                {
                    Name = x.Name,
                    IsEnabled = x.IsEnabled,
                    ScanInterval = x.ScanInterval,
                    OffHoursScanInterval = x.OffHoursScanInterval,
                    DominoServerA = x.DominoServerA,
                    DominoServerB = x.DominoServerB,
                    DominoServerC = x.DominoServerC,
                    Category = x.Category,
                    DominoServerAFileMask = x.DominoServerAFileMask,
                    DominoServerBFileMask = x.DominoServerBFileMask,
                    DominoServerCFileMask = x.DominoServerCFileMask,
                    DominoServerAExcludeFolders = x.DominoServerAExcludeFolders,
                    DominoServerBExcludeFolders = x.DominoServerBExcludeFolders,
                    DominoServerCExcludeFolders = x.DominoServerCExcludeFolders,
                    Id = x.Id,
                    DifferenceThreshold = x.DifferenceThreshold


                }).ToList().OrderBy(x => x.DominoServerA).OrderBy(x => x.DominoServerB);

                Response = Common.CreateResponse(result);
                // Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Delete Server Credentials falied .\n Error Message :");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting Notes database replica has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// Updates Notes Database Replica
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_notes_database_replica")]
        public APIResponse UpdateNotesDatabaseReplica([FromBody]NotesDatabaseReplicaModel notesDatabaseReplica)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> filterExpression;
                if (string.IsNullOrEmpty(notesDatabaseReplica.Id))
                {
                    filterExpression = (p => p.Name == notesDatabaseReplica.Name && p.Type== "Notes Database Replica");
                }
                else
                {
                    filterExpression = (p => p.Name == notesDatabaseReplica.Name && p.Id != notesDatabaseReplica.Id && p.Type == "Notes Database Replica");
                }
                var existedData = serverOtherRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();
                if (existedData == null)
                {
                    if (string.IsNullOrEmpty(notesDatabaseReplica.Id))
                    {
                        ServerOther notesDatabase = new ServerOther
                        {
                            DominoServerA = notesDatabaseReplica.DominoServerA,
                            DominoServerAFileMask = notesDatabaseReplica.DominoServerAFileMask,
                            DominoServerAExcludeFolders = notesDatabaseReplica.DominoServerAExcludeFolders,
                            DominoServerB = notesDatabaseReplica.DominoServerB,
                            DominoServerBFileMask = notesDatabaseReplica.DominoServerBFileMask,
                            DominoServerBExcludeFolders = notesDatabaseReplica.DominoServerBExcludeFolders,
                            DominoServerC = notesDatabaseReplica.DominoServerC,
                            DominoServerCFileMask = notesDatabaseReplica.DominoServerCFileMask,
                            DominoServerCExcludeFolders = notesDatabaseReplica.DominoServerCExcludeFolders,
                            DifferenceThreshold = notesDatabaseReplica.DifferenceThreshold,
                            Type = "Notes Database Replica",
                            Name = notesDatabaseReplica.Name,
                            IsEnabled = notesDatabaseReplica.IsEnabled,
                            Category = notesDatabaseReplica.Category,
                            ScanInterval = notesDatabaseReplica.ScanInterval,
                            OffHoursScanInterval = notesDatabaseReplica.OffHoursScanInterval
                        };
                        string id = serverOtherRepository.Insert(notesDatabase);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Notes database replica inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<ServerOther> filterDefination = Builders<ServerOther>.Filter.Where(p => p.Id == notesDatabaseReplica.Id);
                        var updateDefination = serverOtherRepository.Updater.Set(p => p.DominoServerA, notesDatabaseReplica.DominoServerA)
                            .Set(p => p.DominoServerAFileMask, notesDatabaseReplica.DominoServerAFileMask)
                            .Set(p => p.DominoServerAExcludeFolders, notesDatabaseReplica.DominoServerAExcludeFolders)
                            .Set(p => p.DominoServerB, notesDatabaseReplica.DominoServerB)
                            .Set(p => p.DominoServerBFileMask, notesDatabaseReplica.DominoServerBFileMask)
                            .Set(p => p.DominoServerBExcludeFolders, notesDatabaseReplica.DominoServerBExcludeFolders)
                            .Set(p => p.DominoServerC, notesDatabaseReplica.DominoServerC)
                            .Set(p => p.DominoServerCFileMask, notesDatabaseReplica.DominoServerCFileMask)
                            .Set(p => p.DominoServerCExcludeFolders, notesDatabaseReplica.DominoServerCExcludeFolders)
                            .Set(p => p.Name, notesDatabaseReplica.Name)
                            .Set(p => p.IsEnabled, notesDatabaseReplica.IsEnabled)
                            .Set(p => p.Category, notesDatabaseReplica.Category)
                            .Set(p => p.ScanInterval, notesDatabaseReplica.ScanInterval)
                            .Set(p => p.OffHoursScanInterval, notesDatabaseReplica.OffHoursScanInterval)
                            .Set(p => p.DifferenceThreshold, notesDatabaseReplica.DifferenceThreshold);
                        var result = serverOtherRepository.Update(filterDefination, updateDefination);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Notes database replica updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "This name already exists");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes database replica update has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete("notes_database_replica/{Id}")]
        public APIResponse DeleteNotesDatabaseReplica(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Notes database replica deleted sucessfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes database replica deletion has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion

        #region IBM Domino Settings
        /// <summary>
        /// Returns IBM Domino Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_ibm_domino_settings")]
        public APIResponse GetIbmDominoSettings()
        {
            try
            {

                nameValueRepository = new Repository<NameValue>(ConnectionString);
                var result = nameValueRepository.All()
                                              .Select(x => new
                                              {
                                                  Name = x.Name,
                                                  Value = x.Value
                                              }).ToList();

                var notesProgramDirectory = result.Where(x => x.Name == "Notes Program Directory").Select(x => x.Value).FirstOrDefault();
                var notesUserID = result.Where(x => x.Name == "Notes User ID").Select(x => x.Value).FirstOrDefault();
                var notesIni = result.Where(x => x.Name == "Notes.ini").Select(x => x.Value).FirstOrDefault();
                var password = result.Where(x => x.Name == "Password").Select(x => x.Value).FirstOrDefault();
                if (password != null)
                    password = "****";

                var enableExJournal = result.Where(x => x.Name == "Enable ExJournal").Select(x => x.Value).FirstOrDefault();
                var enableDominoConsoleCommands = result.Where(x => x.Name == "Enable Domino Console Commands").Select(x => x.Value).FirstOrDefault();
                var exJournalthreshold = result.Where(x => x.Name == "ExJournal Threshold").Select(x => x.Value).FirstOrDefault();
                var consecutiveTelnet = result.Where(x => x.Name == "ConsecutiveTelnet").Select(x => x.Value).FirstOrDefault();
                return Common.CreateResponse(new DominoSettingsModel
                {
                    NotesProgramDirectory = notesProgramDirectory,
                    NotesUserID = notesUserID,
                    NotesIni = notesIni,
                    NotesPassword = password,
                    EnableExJournal = Convert.ToBoolean(enableExJournal),
                    EnableDominoConsoleCommands = Convert.ToBoolean(enableDominoConsoleCommands),
                    ExJournalThreshold = exJournalthreshold,
                    ConsecutiveTelnet = consecutiveTelnet
                });
            }
            catch (Exception exception)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting IBM Domino settings has failed.\n Error Message :" + exception.Message);
            }
            return Response;

        }
        /// <summary>
        /// Updates IBM Domino Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_ibm_domino_settings")]
        public APIResponse UpdateIbmDominoSettings([FromBody]DominoSettingsModel dominoSettings)
        {
            try
            {
                FilterDefinition<NameValue> filterDefination;

                try
                {
                    if (dominoSettings.IsModified)
                    {
                        bool updated = false;
                        byte[] MyPass;

                        VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                        MyPass = mySecrets.Encrypt(dominoSettings.NotesPassword);

                        System.Text.StringBuilder newstr = new System.Text.StringBuilder();
                        foreach (byte b in MyPass)
                        {
                            newstr.AppendFormat("{0}, ", b);
                        }
                        string bytepwd = newstr.ToString();
                        int n = bytepwd.LastIndexOf(", ");
                        bytepwd = bytepwd.Substring(0, n);
                        var password = new List<NameValue> { new NameValue { Name = "Password", Value = bytepwd}
                        };
                        var passwordResult = Common.SaveNameValues(password);
                        Response = Common.CreateResponse(passwordResult, Common.ResponseStatus.Success.ToDescription(), "Notes password updated successfully");
                    }
                    else
                    {
                        var ibmDominoSettings = new List<NameValue> { new NameValue { Name = "Notes Program Directory", Value = dominoSettings.NotesProgramDirectory },
                            new NameValue { Name = "Notes User ID", Value = dominoSettings.NotesUserID },
                            new NameValue { Name = "Notes.ini", Value = dominoSettings.NotesIni},
                            new NameValue { Name = "Enable Domino Console Commands", Value = Convert.ToString(dominoSettings.EnableDominoConsoleCommands)},
                            new NameValue { Name = "Enable ExJournal", Value =  Convert.ToString(dominoSettings.EnableExJournal)},
                            new NameValue { Name = "ExJournal Threshold", Value = dominoSettings.ExJournalThreshold},
                            new NameValue { Name = "ConsecutiveTelnet", Value = dominoSettings.ConsecutiveTelnet}
                        };
                        var result = Common.SaveNameValues(ibmDominoSettings);
                        //  Response = Common.CreateResponse(result);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "IBM Domino settings updated successfully.");
                    }
                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving IBM Domino settings has failed.\n Error Message :" + exception.Message);
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving IBM Domino settings has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }
        #endregion

        #region Notes Databases

        /// <summary>
        ///get the notes databases data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpGet("get_notes_databases")]
        public APIResponse GetAllNotesDatabases(string deviceId)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type == "Notes Database" && (deviceId == null || x.Id == deviceId)).Select(x => new NotesDatabaseModel
                {
                    Id = x.Id,
                    // DominoServerId = x.DominoServerId,
                    Name = x.Name,
                    IsEnabled = x.IsEnabled,
                    ScanInterval = x.ScanInterval,
                    OffHoursScanInterval = x.OffHoursScanInterval,
                    RetryInterval = x.RetryInterval,
                    DominoServerName = x.DominoServerName,
                    DatabaseFileName = x.DatabaseFileName,
                    TriggerType = x.TriggerType,
                    TriggerValue = x.TriggerValue,
                    InitiateReplication = x.InitiateReplication,
                    ReplicationDestination = x.ReplicationDestination


                }).ToList().OrderBy(x => x.DominoServerName);

                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Deletion of server credentials has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///saves the notes databases data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpPut("save_notes_databases")]
        public APIResponse UpdateNotesDatabase([FromBody]NotesDatabaseModel notesDatabase)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> filterExpression;
                if (string.IsNullOrEmpty(notesDatabase.Id))
                {
                    filterExpression = (p => p.Name == notesDatabase.Name && p.Type == "Notes Database");
                }
                else
                {
                    filterExpression = (p => p.Name == notesDatabase.Name && p.Id != notesDatabase.Id && p.Type == "Notes Database");
                }
                var existsData = serverOtherRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();

                if (string.IsNullOrEmpty(existsData))
                {
                    if (string.IsNullOrEmpty(notesDatabase.Id))
                    {
                        ServerOther notesDatabases = new ServerOther
                        {
                            DominoServerName = notesDatabase.DominoServerName,
                            Name = notesDatabase.Name,
                            DatabaseFileName = notesDatabase.DatabaseFileName,
                            Type = "Notes Database",
                            TriggerType = notesDatabase.TriggerType,
                            ScanInterval = notesDatabase.ScanInterval,
                            OffHoursScanInterval = notesDatabase.OffHoursScanInterval,
                            IsEnabled = notesDatabase.IsEnabled,
                            RetryInterval = notesDatabase.RetryInterval,
                            TriggerValue = notesDatabase.TriggerValue,
                            InitiateReplication = notesDatabase.InitiateReplication,
                            ReplicationDestination = notesDatabase.ReplicationDestination
                            //DominoServerId = notesDatabase.DominoServerId
                        };
                        string id = serverOtherRepository.Insert(notesDatabases);
                        //2/24/2017 NS added for VSPLUS-3506
                        Licensing licensing = new Licensing();
                        licensing.refreshServerCollectionWrapper();
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Notes database inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<ServerOther> filterDefination = Builders<ServerOther>.Filter.Where(p => p.Id == notesDatabase.Id);
                        var updateDefination = serverOtherRepository.Updater.Set(p => p.DominoServerName, notesDatabase.DominoServerName)
                            .Set(p => p.Name, notesDatabase.Name)
                            .Set(p => p.DatabaseFileName, notesDatabase.DatabaseFileName)
                            .Set(p => p.TriggerType, notesDatabase.TriggerType)
                            .Set(p => p.ScanInterval, notesDatabase.ScanInterval)
                            .Set(p => p.OffHoursScanInterval, notesDatabase.OffHoursScanInterval)
                            .Set(p => p.IsEnabled, notesDatabase.IsEnabled)
                            .Set(p => p.TriggerValue, notesDatabase.TriggerValue)
                            .Set(p => p.RetryInterval, notesDatabase.RetryInterval)
                            .Set(p => p.InitiateReplication, notesDatabase.InitiateReplication)
                            .Set(p => p.ReplicationDestination, notesDatabase.ReplicationDestination);
                        var result = serverOtherRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Notes database updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This name already exists. Please enter a different one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes database creation has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        /// <summary>
        ///delete  the notes databases data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpDelete("delete_notes_database/{Id}")]
        public APIResponse DeleteNotesDatabase(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Notes database record deleted succesfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of a Notes database record has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion

        #region IBM Domino Server Tasks
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>

        [HttpGet("get_server_task_definiton")]
        public APIResponse GetServerTask()
        {
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                var result = dominoservertasksRepository.All().Select(x => new ServerTaskDefinitionModel
                {
                    Id = x.Id,
                    TaskName = x.TaskName,
                    LoadString = x.LoadString,
                    ConsoleString = x.ConsoleString,
                    FreezeDetect = x.FreezeDetect,
                    IdleString = x.IdleString,
                    MaxBusyTime = x.MaxBusyTime,
                    RetryCount = x.RetryCount

                }).ToList();
                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Geting domino server task information has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="servertask"></param>
        /// <returns></returns>
        [HttpPut("save_server_task_definition")]
        public APIResponse UpdateServerTaskDefinition([FromBody]ServerTaskDefinitionModel servertask)
        {
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                if (string.IsNullOrEmpty(servertask.Id))
                {
                    DominoServerTasks servertaskDef = new DominoServerTasks { TaskName = servertask.TaskName, LoadString = servertask.LoadString, ConsoleString = servertask.ConsoleString, FreezeDetect = servertask.FreezeDetect, IdleString = servertask.IdleString, MaxBusyTime = servertask.MaxBusyTime, RetryCount = servertask.RetryCount };
                    string id = dominoservertasksRepository.Insert(servertaskDef);
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Domino server task definition inserted successfully");
                }
                else
                {
                    FilterDefinition<DominoServerTasks> filterDefination = Builders<DominoServerTasks>.Filter.Where(p => p.Id == servertask.Id);
                    var updateDefination = dominoservertasksRepository.Updater.Set(p => p.TaskName, servertask.TaskName)
                                                             .Set(p => p.LoadString, servertask.LoadString)
                                                             .Set(p => p.ConsoleString, servertask.ConsoleString)
                                                             .Set(p => p.FreezeDetect, servertask.FreezeDetect)
                                                             .Set(p => p.IdleString, servertask.IdleString)
                                                             .Set(p => p.MaxBusyTime, servertask.MaxBusyTime)
                                                             .Set(p => p.RetryCount, servertask.RetryCount);

                    var result = dominoservertasksRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Domino server task definition updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving Domino server task has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_server_task_definition/{id}")]
        public APIResponse DeleteServerTaskDefinition(string id)
        {
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                Expression<Func<DominoServerTasks, bool>> expression = (p => p.Id == id);
                dominoservertasksRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server task deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of a server task has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion

        #region Schedule Reports

        [HttpGet("get_scheduled_reports")]
        public APIResponse GetScheduledReportsList()
        {

            List<dynamic> result = new List<dynamic>();

            try
            {
                result = GetScheduledReport();
                Response = Common.CreateResponse(result);
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
           }

        private List<dynamic> GetScheduledReport()
        {
            List<dynamic> result_disp = new List<dynamic>();
            List<bool> is_selected_event = new List<bool>();
            List<string> event_ids = new List<string>();
            List<ReportTitle> report = new List<ReportTitle>();
            try
            {
                schedulereportsrepository = new Repository<ScheduledReports>(ConnectionString);
                var schedulereports = schedulereportsrepository.All().ToList();
                sitemaprepository = new Repository<VSNext.Mongo.Entities.SiteMap>(ConnectionString);
                foreach (var schedulereport in schedulereports)
                {

                    result_disp.Add(new ScheduledReportsModel
                    {
                        Id = schedulereport.Id,
                        ReportName = schedulereport.ReportName,
                        ReportSubject = schedulereport.ReportSubject,
                        ReportBody = schedulereport.ReportBody,
                        Frequency = schedulereport.Frequency,
                        CopyTo = schedulereport.CopyTo,
                        SendTo = schedulereport.SendTo,
                        BlindCopyTo = schedulereport.BlindCopyTo,
                        FileFormat = schedulereport.FileFormat,
                        FrequencyDayList = schedulereport.FrequencyDayList,
                        Repeat = schedulereport.Repeat,
                        SelectedReports = schedulereport.SelectedReports
                      
                    });
                }
                var reportTitleMapper = new Dictionary<string, string>();
                var categorybyreport = new Dictionary<string, List<string>>();
                reportTitleMapper.Add("CPU / Memory / Disk Utilization", "disk_reports");
                reportTitleMapper.Add("Financial", "financial_reports");
                reportTitleMapper.Add("IBM Connections", "connections_reports");
                reportTitleMapper.Add("IBM Domino", "domino_reports");
                reportTitleMapper.Add("IBM Traveler", "traveler_reports");
                reportTitleMapper.Add("IBM WebSphere", "websphere_reports");
                reportTitleMapper.Add("IBM Sametime", "sametime_reports");
                reportTitleMapper.Add("Microsoft Exchange", "exchange_reports");
                reportTitleMapper.Add("Mail", "mail_reports");
                reportTitleMapper.Add("Mobile Devices", "mobile_users");
                reportTitleMapper.Add("Office 365", "office365_reports");
                reportTitleMapper.Add("Servers & Configuration", "server_reports");

                var sitemapList = sitemaprepository.All().ToList();
                foreach (var reportitem in reportTitleMapper)
                {
                   
                    var node = sitemapList.Find(x => x.Id == reportitem.Value).Nodes;
                    var nodeTitles = node.Select(x => x.Title).ToList();
                    categorybyreport.Add(reportitem.Key, nodeTitles);
                }


                foreach (var currentparent in categorybyreport)
                {
                    foreach (var currchild in currentparent.Value)
                    {
                        ReportTitle temp = new ReportTitle();
                        temp.IsSelected = false;
                        temp.ReportTitles = currentparent.Key;
                        temp.ReportCategory = currchild;
                        report.Add(temp);
                    }


                }
                return new List<dynamic>() {
                    result_disp,
                    report

                };
            }

            catch (Exception exception)
            {
                throw exception;
        }
        }
        [HttpPut("save_scheduled_reports")]
        public APIResponse UpdateScheduledReports([FromBody]ScheduledReportsModel ScheduledReports)
        {
            List<dynamic> result_disp = new List<dynamic>();
            try
            {
                schedulereportsrepository = new Repository<ScheduledReports>(ConnectionString);
                if (string.IsNullOrEmpty(ScheduledReports.Id))
                {
                    ScheduledReports ScheduledDef = new ScheduledReports {
                        Id = ScheduledReports.Id,
                        ReportName = ScheduledReports.ReportName,
                        ReportSubject = ScheduledReports.ReportSubject,
                        ReportBody = ScheduledReports.ReportBody,
                        Frequency = ScheduledReports.Frequency,
                        SendTo = ScheduledReports.SendTo,
                        CopyTo = ScheduledReports.CopyTo,
                        BlindCopyTo = ScheduledReports.BlindCopyTo,
                        FileFormat = ScheduledReports.FileFormat,
                        FrequencyDayList = ScheduledReports.FrequencyDayList,
                        Repeat = String.IsNullOrWhiteSpace(ScheduledReports.Repeat) ? "0" : ScheduledReports.Repeat,
                        SelectedReports = ScheduledReports.SelectedReports,
                        
                    };
                    string id = schedulereportsrepository.Insert(ScheduledDef);
                    result_disp = GetScheduledReport();
                    Response = Common.CreateResponse(result_disp, Common.ResponseStatus.Success.ToDescription(), "Scheduled Reports inserted successfully");
                }
                else
                {
                    FilterDefinition<ScheduledReports> filterDefination = Builders<ScheduledReports>.Filter.Where(p => p.Id == ScheduledReports.Id);
                    var updateDefination = schedulereportsrepository.Updater.Set(p => p.ReportName, ScheduledReports.ReportName)
                                                             .Set(p => p.ReportSubject, ScheduledReports.ReportSubject)
                                                             .Set(p => p.ReportBody, ScheduledReports.ReportBody)
                                                             .Set(p => p.Frequency, ScheduledReports.Frequency)
                                                             .Set(p => p.SendTo, ScheduledReports.SendTo)
                                                             .Set(p => p.CopyTo, ScheduledReports.CopyTo)
                                                             .Set(p => p.BlindCopyTo, ScheduledReports.BlindCopyTo)
                                                             .Set(p => p.FileFormat, ScheduledReports.FileFormat)
                                                             .Set(p => p.FrequencyDayList, ScheduledReports.FrequencyDayList)
                                                             .Set(p => p.Repeat, ScheduledReports.Repeat)
                                                              .Set(p => p.SelectedReports, ScheduledReports.SelectedReports);
                    var result = schedulereportsrepository.Update(filterDefination, updateDefination);
                    result_disp = GetScheduledReport();
                    Response = Common.CreateResponse(result_disp, Common.ResponseStatus.Success.ToDescription(), "Scheduled Reports updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving Scheduled Reports has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_scheduled_reports/{id}")]
        public APIResponse DeleteScheduledReports(string id)
        {
            try
            {
                schedulereportsrepository = new Repository<ScheduledReports>(ConnectionString);
                Expression<Func<ScheduledReports, bool>> expression = (p => p.Id == id);
                schedulereportsrepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Scheduled Report has been deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of a Scheduled Report has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion


        #region Log File Scanning
        /// <summary>
        /// Get Log Scanning 
        /// </summary>
        /// <author>Swathi </author>     
        /// <returns></returns>
        [HttpGet("get_log_scaning")]
        public APIResponse GetAllLogFileScanning()
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type == "Domino Log Scanning").Select(x => new LogFileScanning
                {
                    Id = x.Id,
                    Name = x.Name,


                }).ToList().OrderBy(x => x.Name);

                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting log file scanning information has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// Get Event Log Scanning 
        /// </summary>
        /// <author>Swathi </author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get_event_log_scaning/{id}")]
        public APIResponse GetEventLogScanning(string id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                List<Models.Configurator.LogFile> service = new List<Models.Configurator.LogFile>();
                if (id != "-1")
                {
                    var serverOther = serverOtherRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == id);
                    if (serverOther != null)
                    {
                        var devicename = serverOther.Name;
                        var result = serverOther.LogFileKeywords;
                        var servers = serverOther.LogFileServers;

                        service = new List<Models.Configurator.LogFile>();
                        foreach (LogFileKeyword task in result)
                        {
                            service.Add(new Models.Configurator.LogFile
                            {
                                Keyword = task.Keyword,
                                Exclude = task.Exclude,
                                OneAlertPerDay = task.OneAlertPerDay,
                                ScanLog = task.ScanLog,
                                ScanAgentLog = task.ScanAgentLog,
                                Id = task.EventId


                            });

                        }

                        Response = Common.CreateResponse(new { devicename = devicename, result = service, servers = servers });
                    }

                }
                else
                {
                    Response = Common.CreateResponse(new { devicename = string.Empty, result = service, servers = new List<string>() });
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting event log file scanning information has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        /// Save Event Log Scanning 
        /// </summary>
        /// <author>Swathi </author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("save_log_file_servers/{id}")]
        public APIResponse UpdateLogFileServers([FromBody] DeviceSettings devicesettings, [FromBody] VitalSigns.API.Models.Configurator.LogFile eventlog, string id)
        {
            try
            {
                if (devicesettings.Setting == null && devicesettings.Devices == null && devicesettings.Value == null)
                {
                    Response = Common.CreateResponse(false,Common.ResponseStatus.Success.ToDescription(), "Keyword created successfully");
                }
                else
                {
                    serverOtherRepository = new Repository<ServerOther>(ConnectionString);

                    string settingValue = Convert.ToString(devicesettings.Value);
                    if(string.IsNullOrEmpty(settingValue))
                    {
                        Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "Please enter a Domino event definition");

                    }
                    else
                    {
                        var devicesList = ((Newtonsoft.Json.Linq.JArray)devicesettings.Devices).ToObject<List<string>>();
                        var logfiles = ((Newtonsoft.Json.Linq.JArray)devicesettings.Setting).ToObject<List<Models.Configurator.LogFile>>();
                        //  var server = serverOtherRepository.Get(id);
                        Expression<Func<ServerOther, bool>> filterExpression;
                        if (id == ("-1"))
                        {
                            filterExpression = (p => p.Name == settingValue);

                        }
                        else
                        {
                            filterExpression = (p => p.Name == settingValue && p.Id != id);

                        }
                        var existsData = serverOtherRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();
                        // var existEventlog = serverOtherRepository.Collection.AsQueryable().Where(x => x.Name == settingValue).FirstOrDefault();
                        if (existsData != settingValue)
                        {

                            UpdateDefinition<ServerOther> updateDefinition = null;
                            List<LogFileKeyword> logscannings = new List<LogFileKeyword>();
                            if (id == ("-1"))
                            {
                                foreach (var logfile in logfiles)
                                {
                                    if (logfile.Id != "-1")
                                    {
                                        logscannings.Add(new LogFileKeyword
                                        {
                                            EventId = ObjectId.GenerateNewId().ToString(),
                                            Keyword = logfile.Keyword,
                                            Exclude = logfile.Exclude,
                                            OneAlertPerDay = logfile.OneAlertPerDay,
                                            ScanLog = logfile.ScanLog,
                                            ScanAgentLog = logfile.ScanAgentLog
                                        });
                                    }
                                }



                                ServerOther logscanserver = new ServerOther { Name = settingValue, Type = "Domino Log Scanning", LogFileKeywords = logscannings, LogFileServers = devicesList };
                                string newid = serverOtherRepository.Insert(logscanserver);
                                //2/24/2017 NS added for VSPLUS-3506
                                Licensing licensing = new Licensing();
                                licensing.refreshServerCollectionWrapper();
                                Response = Common.CreateResponse(newid, Common.ResponseStatus.Success.ToDescription(), "Event definition inserted successfully");
                            }
                            if (id != ("-1"))
                            {
                                if (devicesList.Count() > 0)
                                {
                                    if (!string.IsNullOrEmpty(settingValue))
                                    {

                                        foreach (var logfile in logfiles)
                                        {

                                            logscannings.Add(new LogFileKeyword
                                            {
                                                // EventId = ObjectId.GenerateNewId().ToString(),
                                                Keyword = logfile.Keyword,
                                                Exclude = logfile.Exclude,
                                                OneAlertPerDay = logfile.OneAlertPerDay,
                                                ScanLog = logfile.ScanLog,
                                                ScanAgentLog = logfile.ScanAgentLog
                                            });
                                        }
                                        FilterDefinition<ServerOther> filterDefination = Builders<ServerOther>.Filter.Where(p => p.Id == id);
                                        var updateDefination = serverOtherRepository.Updater.Set(p => p.Name, settingValue)
                                                                                 .Set(p => p.LogFileServers, devicesList)
                                                                                 .Set(p => p.LogFileKeywords, logscannings)
                                                                                  .Set(p => p.Type, "Domino Log Scanning");

                                        var result = serverOtherRepository.Collection.UpdateMany(filterDefination, updateDefination);
                                        //2/24/2017 NS added for VSPLUS-3506
                                        Licensing licensing = new Licensing();
                                        licensing.refreshServerCollectionWrapper();
                                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Event definition updated successfully");

                                    }

                                }
                            }


                        }
                        else if (existsData == settingValue)
                        {
                            Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "The event definition " + "" + existsData + " " + "already exists. Please enter a different one.");
                        }

                        if (logfiles.Count == 0)
                        {
                            Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "Please create at least one Domino event log entry.");
                        }
                    }

                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Domino event definition creation has failed.\n Error Message :" + exception.Message);
            }
            return Response;

        }

        /// <summary>
        /// Delete Domino Event Definition Scanning 
        /// </summary>
        /// <author>Swathi </author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_log_file_scanning/{Id}")]
        public APIResponse DeleteLogFileScanning(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Domino event definition deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of Domino event definition has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// Delete Event Log File Scanning 
        /// </summary>
        /// <author>Swathi </author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_event_log_file_scanning/{deviceId}/{id}")]
        public APIResponse DeleteEventLogFileScanning(string deviceId, string id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);

                var server = serverOtherRepository.Get(deviceId);
                var dominoServerTasks = server.LogFileKeywords;
                if(id=="false")
                {
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Domino event log scanning deleted successfully");
                }
                var serverTaskDelete = dominoServerTasks.FirstOrDefault(x => x.EventId == id);
                if (serverTaskDelete != null)
                {
                    dominoServerTasks.Remove(serverTaskDelete);
                    var updateDefinition = serverOtherRepository.Updater.Set(p => p.LogFileKeywords, dominoServerTasks);
                    var result = serverOtherRepository.Update(server, updateDefinition);
                    //2/24/2017 NS added for VSPLUS-3506
                    Licensing licensing = new Licensing();
                    licensing.refreshServerCollectionWrapper();
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Domino event log scanning deleted successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Deletion of event log file has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Traveller Data Store
        /// <summary>
        /// 
        /// </summary>
        /// <author>Sowmya</author>
        /// <returns></returns>

        [HttpGet("get_travelerdatastore")]
        public APIResponse GetAllTravelerDataStore()
        {
            try
            {
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                var travellerData = travelerdatastoreRepository.All().Select(x => new TravelerDataStoresModel
                {
                    Id = x.Id,
                    TravelerServicePoolName = x.TravelerServicePoolName,
                    DeviceName = x.DeviceName,
                    DataStore = x.DataStore,
                    DatabaseName = x.DatabaseName,
                    Port = x.Port,
                    UserName = x.UserName,
                    Password = x.Password,
                    IntegratedSecurity = x.IntegratedSecurity,
                    TestScanServer = x.TestScanServer
                    //UsedByServers = x.UsedByServers

                }).ToList().OrderBy(x => x.DeviceName);
                statusRepository = new Repository<Status>(ConnectionString);
                var travelerServers = statusRepository.Collection.AsQueryable().Where(x => x.SecondaryRole.Contains("Traveler")).Select(x => new ComboBoxListItem { DisplayText = x.DeviceName, Value = x.Id }).OrderBy(x => x.DisplayText).ToList();
                Response = Common.CreateResponse(new { travellerData = travellerData, travelerServers = travelerServers });
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting traveler data has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="travelerdatas"></param>
        /// <returns></returns>
        [HttpPut("save_traveler_data_store")]
        public APIResponse UpdateTravelerDataStore([FromBody]TravelerDataStoresModel travelerdatas)
        {
            try
            {
                byte[] password;
                string bytepwd = "";
                if (!string.IsNullOrEmpty(travelerdatas.Password))
                {
                    password = tripleDes.Encrypt(travelerdatas.Password);

                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (byte b in password)
                    {
                        stringBuilder.AppendFormat("{0}, ", b);
                    }
                    bytepwd = stringBuilder.ToString();
                    int n = bytepwd.LastIndexOf(", ");
                    bytepwd = bytepwd.Substring(0, n);
                }
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                if (travelerdatas.IsModified)
                {        
                    FilterDefinition<TravelerDTS> filterDefination = Builders<TravelerDTS>.Filter.Where(p => p.Id == travelerdatas.Id);
                    var updateDefination = travelerdatastoreRepository.Updater
                        .Set(p => p.Password, bytepwd);
                    var result = travelerdatastoreRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Password updated successfully");
                }
                else
                {
                    if (string.IsNullOrEmpty(travelerdatas.Id))
                    {
                        TravelerDTS travelerds = new TravelerDTS {
                            TravelerServicePoolName = travelerdatas.TravelerServicePoolName,
                            DeviceName = travelerdatas.DeviceName,
                            DataStore = travelerdatas.DataStore,
                            DatabaseName = travelerdatas.DatabaseName,
                            Port = travelerdatas.Port,
                            UserName = travelerdatas.UserName,
                            Password = bytepwd,
                            IntegratedSecurity = travelerdatas.IntegratedSecurity,
                            TestScanServer = travelerdatas.TestScanServer };//, UsedByServers = travelerdatas.UsedByServers };
                        string id = travelerdatastoreRepository.Insert(travelerds);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Traveler data store inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<TravelerDTS> filterDefination = Builders<TravelerDTS>.Filter.Where(p => p.Id == travelerdatas.Id);
                        var updateDefination = travelerdatastoreRepository.Updater.Set(p => p.TravelerServicePoolName, travelerdatas.TravelerServicePoolName)
                            .Set(p => p.DeviceName, travelerdatas.DeviceName)
                            .Set(p => p.DataStore, travelerdatas.DataStore)
                            .Set(p => p.DatabaseName, travelerdatas.DatabaseName)
                            .Set(p => p.Port, travelerdatas.Port)
                            .Set(p => p.UserName, travelerdatas.UserName)
                            .Set(p => p.IntegratedSecurity, travelerdatas.IntegratedSecurity)
                            .Set(p => p.TestScanServer, travelerdatas.TestScanServer);
                        //.Set(p => p.UsedByServers, travelerdatas.UsedByServers);
                        var result = travelerdatastoreRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Traveler data store updated successfully");
                    }
                }               
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving traveler data store has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_traveler_data_store/{id}")]
        public APIResponse DeleteTravelerDataStore(string id)
        {
            try
            {
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                Expression<Func<TravelerDTS, bool>> expression = (p => p.Id == id);
                travelerdatastoreRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Traveler data store deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of Traveler data has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #endregion

        #region Alerts

        #region Alert Settings

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="listAlertSettings"></param>
        /// <returns></returns>

        [HttpPut("save_alert_settings")]
        public APIResponse UpdateIbmAlertSettings([FromBody]RecurringEvents listAlertSettings)
        {
            //Console.WriteLine("inside function");
            try
            {
                FilterDefinition<EventsMaster> filterDef;
                UpdateDefinition<EventsMaster> updateEvents;
                AlertSettingsModel alertSettings = listAlertSettings.AlertSettings;
                List<string> selectedEvents = listAlertSettings.SelectedEvents;

                if (alertSettings.PrimaryModified || alertSettings.SecondaryModified)
                {
                    byte[] password;
                    string bytepwd = "";

                    if (alertSettings.PrimaryModified)
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(alertSettings.PrimaryPwd))
                            {
                                password = tripleDes.Encrypt(alertSettings.PrimaryPwd);

                                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                foreach (byte b in password)
                                {
                                    stringBuilder.AppendFormat("{0}, ", b);
                                }
                                bytepwd = stringBuilder.ToString();
                                int n = bytepwd.LastIndexOf(", ");
                                bytepwd = bytepwd.Substring(0, n);
                                var alertData = new List<NameValue> { new NameValue { Name = "Primarypwd", Value = bytepwd } };
                                var result = Common.SaveNameValues(alertData);
                                Response = Common.CreateResponse(true, "Success", "Password was successfully updated");
                            }
                            else
                            {
                                var alertData = new List<NameValue> { new NameValue { Name = "Primarypwd", Value = "" } };
                                var result = Common.SaveNameValues(alertData);
                                Response = Common.CreateResponse(true, "Success", "Password was successfully cleared");
                            }
                        }
                        catch (Exception exception)
                        {
                            Response = Common.CreateResponse(null, "Error", "Saving a password has failed.\n Error Message :" + exception.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            if (!string.IsNullOrEmpty(alertSettings.SecondaryPwd))
                            {
                                password = tripleDes.Encrypt(alertSettings.SecondaryPwd);

                                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                                foreach (byte b in password)
                                {
                                    stringBuilder.AppendFormat("{0}, ", b);
                                }
                                bytepwd = stringBuilder.ToString();
                                int n = bytepwd.LastIndexOf(", ");
                                bytepwd = bytepwd.Substring(0, n);
                                var alertData = new List<NameValue> { new NameValue { Name = "SecondaryPwd", Value = bytepwd } };
                                var result = Common.SaveNameValues(alertData);
                                Response = Common.CreateResponse(true, "Success", "Password was successfully updated");
                            }
                            else
                            {
                                var alertData = new List<NameValue> { new NameValue { Name = "SecondaryPwd", Value = "" } };
                                var result = Common.SaveNameValues(alertData);
                                Response = Common.CreateResponse(true, "Success", "Password was successfully cleared");
                            }
                        }
                        catch (Exception exception)
                        {
                            Response = Common.CreateResponse(null, "Error", "Saving a password has failed.\n Error Message :" + exception.Message);
                        }
                    }
                }
                else
                {
                    try
                    {
                        var alertData = new List<NameValue> { new NameValue { Name = "PrimaryHostName", Value = alertSettings.PrimaryHostName },
                        new NameValue { Name = "PrimaryFrom", Value = alertSettings.PrimaryForm },
                        new NameValue { Name = "PrimaryUserId", Value = alertSettings.PrimaryUserId},
                        new NameValue { Name = "PrimaryPort", Value =Convert.ToString(alertSettings.PrimaryPort)},
                        new NameValue { Name = "SmsForm", Value =  alertSettings.SmsForm},
                        new NameValue { Name = "PrimaryAuth", Value =Convert.ToString(alertSettings.PrimaryAuth)},
                        new NameValue { Name = "PrimarySSL", Value = Convert.ToString(alertSettings.PrimarySSL)},
                        new NameValue { Name = "SecondaryHostName", Value =  Convert.ToString(alertSettings.SecondaryHostName)},
                        new NameValue { Name = "SecondaryFrom", Value = alertSettings.SecondaryForm},
                        new NameValue { Name = "SecondaryUserId", Value = alertSettings.SecondaryUserId},
                        new NameValue { Name = "SecondaryPort", Value = alertSettings.SecondaryPort},
                        new NameValue { Name = "SecondaryAuth", Value = Convert.ToString(alertSettings.SecondaryAuth)},
                        new NameValue { Name = "SecondarySSL", Value =  Convert.ToString(alertSettings.SecondarySSL)},
                        new NameValue { Name = "SmsAccountSid", Value = alertSettings.SmsAccountSid},
                        new NameValue { Name = "SmsAuthToken", Value = alertSettings.SmsAuthToken},
                        new NameValue { Name = "EnablePersitentAlerting", Value=alertSettings.EnablePersistentAlerting?"True":"False"},
                        new NameValue { Name = "AlertInterval", Value =Convert.ToString(alertSettings.AlertInterval)},
                        new NameValue { Name = "AlertDuration", Value = Convert.ToString(alertSettings.AlertDuration)},
                        new NameValue { Name = "EnableAlertLimits", Value = (alertSettings.EnableAlertLimits?"True":"False")},
                        new NameValue { Name = "TotalMaximumAlertsPerDefinition", Value = Convert.ToString(alertSettings.TotalMaximumAlertsPerDefinition)},
                        new NameValue {Name = "TotalMaximumAlertsPerDay", Value=Convert.ToString(alertSettings.TotalMaximumAlertsPerDay)},
                        new NameValue { Name = "EnableSNMPTraps",Value=(alertSettings.EnableSNMPTraps?"True":"False")},
                        new NameValue {Name = "HostName", Value= alertSettings.HostName},
                        new NameValue { Name = "AlertAboutRecurrencesOnly",Value=Convert.ToString(alertSettings.AlertAboutRecurrencesOnly)},
                        new NameValue {Name = "NumberOfRecurrences", Value= alertSettings.NumberOfRecurrences.ToString()},
                        new NameValue {Name = "AlertsOn", Value= alertSettings.AlertsOn.ToString()}
                    };
                        var result = Common.SaveNameValues(alertData);
                        eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                        filterDef = eventsMasterRepository.Filter.Eq(x => x.NotificationOnRepeat, true);
                        updateEvents = eventsMasterRepository.Updater.Set(x => x.NotificationOnRepeat, false);
                        eventsMasterRepository.Update(filterDef, updateEvents);
                        if (Convert.ToBoolean(alertSettings.AlertAboutRecurrencesOnly))
                        {
                            foreach (var selectedEvent in selectedEvents)
                            {
                                filterDef = eventsMasterRepository.Filter.And(eventsMasterRepository.Filter.Eq(x => x.Id, selectedEvent));
                                updateEvents = eventsMasterRepository.Updater.Set(x => x.NotificationOnRepeat, true);
                                eventsMasterRepository.Update(filterDef, updateEvents);
                            }
                        }

                        Response = Common.CreateResponse(true, "Success", "Alert settings were successully updated.");
                    }
                    catch (Exception exception)
                    {
                        Response = Common.CreateResponse(null, "Error", "Saving alert settings has failed .\n Error Message :" + exception.Message);
                    }
                }           
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving alert settings has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_alert_settings")]
        public APIResponse GetAlertSettings()
        {
            nameValueRepository = new Repository<NameValue>(ConnectionString);
            var result = nameValueRepository.All()
                                          .Select(x => new
                                          {
                                              Name = x.Name,
                                              Value = x.Value
                                          }).ToList();

            var primaryHostName = result.Where(x => x.Name == "PrimaryHostName").Select(x => x.Value).FirstOrDefault();
            var primaryForm = result.Where(x => x.Name == "PrimaryFrom").Select(x => x.Value).FirstOrDefault();
            var primaryUserId = result.Where(x => x.Name == "PrimaryUserId").Select(x => x.Value).FirstOrDefault();
            var primaryPwd = result.Where(x => x.Name == "Primarypwd").Select(x => x.Value).FirstOrDefault();
            var primaryPort = result.Where(x => x.Name == "PrimaryPort").Select(x => x.Value).FirstOrDefault();

            var primaryAuth = result.Where(x => x.Name == "PrimaryAuth").Select(x => x.Value).FirstOrDefault();
            var primarySSL = result.Where(x => x.Name == "PrimarySSL").Select(x => x.Value).FirstOrDefault();
            // var primaryPwd = result.Where(x => x.Name == "PrimaryPwd").Select(x => x.Value).FirstOrDefault();
            var secondaryHostName = result.Where(x => x.Name == "SecondaryHostName").Select(x => x.Value).FirstOrDefault();
            var secondaryForm = result.Where(x => x.Name == "SecondaryFrom").Select(x => x.Value).FirstOrDefault();

            var secondaryUserId = result.Where(x => x.Name == "SecondaryUserId").Select(x => x.Value).FirstOrDefault();
            var secondaryPwd = result.Where(x => x.Name == "SecondaryPwd").Select(x => x.Value).FirstOrDefault();
            var secondaryPort = result.Where(x => x.Name == "SecondaryPort").Select(x => x.Value).FirstOrDefault();
            var secondaryAuth = result.Where(x => x.Name == "SecondaryAuth").Select(x => x.Value).FirstOrDefault();
            var secondarySSL = result.Where(x => x.Name == "SecondarySSL").Select(x => x.Value).FirstOrDefault();
            var smsAccountSid = result.Where(x => x.Name == "SmsAccountSid").Select(x => x.Value).FirstOrDefault();
            var smsAuthToken = result.Where(x => x.Name == "SmsAuthToken").Select(x => x.Value).FirstOrDefault();
            var smsForm = result.Where(x => x.Name == "SmsForm").Select(x => x.Value).FirstOrDefault();
            var enablePersistentAlerting = result.Where(x => x.Name == "EnablePersitentAlerting").Select(x => x.Value).FirstOrDefault();
            var alertInterval = result.Where(x => x.Name == "AlertInterval").Select(x => x.Value).FirstOrDefault();
            var alertDuration = result.Where(x => x.Name == "AlertDuration").Select(x => x.Value).FirstOrDefault();
            var email = result.Where(x => x.Name == "Email").Select(x => x.Value).FirstOrDefault();
            var enableAlertLimits = result.Where(x => x.Name == "EnableAlertLimits").Select(x => x.Value).FirstOrDefault();
            var totalMaximumAlertsPerDefinition = result.Where(x => x.Name == "TotalMaximumAlertsPerDefinition").Select(x => x.Value).FirstOrDefault();
            var totalMaximumAlertsPerDay = result.Where(x => x.Name == "TotalMaximumAlertsPerDay").Select(x => x.Value).FirstOrDefault();
            var enableSNMPTraps = result.Where(x => x.Name == "EnableSNMPTraps").Select(x => x.Value).FirstOrDefault();
            var hostName = result.Where(x => x.Name == "HostName").Select(x => x.Value).FirstOrDefault();
            var alertAboutRecurrencesOnly = result.Where(x => x.Name == "AlertAboutRecurrencesOnly").Select(x => x.Value).FirstOrDefault();
            var numberOfRecurrences = result.Where(x => x.Name == "NumberOfRecurrences").Select(x => x.Value).FirstOrDefault();
            var alertsOn = result.Where(x => x.Name == "AlertsOn").Select(x => x.Value).FirstOrDefault();
            return Common.CreateResponse(new AlertSettingsModel
            {
                PrimaryHostName = primaryHostName,
                PrimaryForm = primaryForm,
                PrimaryUserId = primaryUserId,
                PrimaryPort = Convert.ToInt32(primaryPort),
                PrimaryAuth = Convert.ToBoolean(primaryAuth),
                PrimarySSL = Convert.ToBoolean(primarySSL),
                PrimaryPwd = primaryPwd,
                SecondaryHostName = secondaryHostName,
                SecondaryForm = secondaryForm,

                SecondaryUserId = secondaryUserId,
                SecondaryPwd = secondaryPwd,
                SecondaryPort = secondaryPort,
                SecondaryAuth = Convert.ToBoolean(secondaryAuth),
                SecondarySSL = Convert.ToBoolean(secondarySSL),
                SmsAccountSid = smsAccountSid,
                SmsAuthToken = smsAuthToken,
                SmsForm = smsForm,

                EnablePersistentAlerting = Convert.ToBoolean(enablePersistentAlerting),
                AlertInterval = alertInterval,
                AlertDuration = alertDuration,
                //EMail = email,
                EnableAlertLimits = Convert.ToBoolean(enableAlertLimits),
                TotalMaximumAlertsPerDay = Convert.ToInt32(totalMaximumAlertsPerDay),
                TotalMaximumAlertsPerDefinition = Convert.ToInt32(totalMaximumAlertsPerDefinition),
                EnableSNMPTraps = Convert.ToBoolean(enableSNMPTraps),

                HostName = hostName,
                AlertAboutRecurrencesOnly = Convert.ToBoolean(alertAboutRecurrencesOnly),
                NumberOfRecurrences = Convert.ToInt32(numberOfRecurrences),

                AlertsOn = Convert.ToBoolean(alertsOn)  
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("events_master_list")]
        public APIResponse GetEventsMasterList()
        {
            try
            {
                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                var result = eventsMasterRepository.All().OrderBy(x => x.DeviceType).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }

            return Response;
        }

        #endregion

        #region View Alerts
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="statdate"></param>
        /// <returns></returns>
        [HttpGet("viewalerts")]
        public APIResponse GetViewalerts(DateTime statdate)
        {
            try
            {
                DateTime? nullDate = null;
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                if (statdate == null)
                {
                    statdate = DateTime.MinValue;
                }
                if (statdate == DateTime.MinValue || statdate.Date == DateTime.Now.Date)
                {
                    statdate = DateTime.Now;
                    var resultList = eventsdetectedRepository.All().Where(x => x.EventDetected.HasValue).ToList();
                    var result = resultList
                        .Select(s => new AlertsModel
                        {
                            DeviceName = (s.Device != "" && s.Device != null) ? s.Device : "System message",
                            DeviceType = s.DeviceType,
                            AlertType = s.EventType,
                            Details = s.Details,
                            EventDetected = s.EventDetected != null ? s.EventDetected.Value : nullDate,
                            EventDetectedSent = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].EventDetectedSent.Value) : nullDate,
                            EventDismissed = s.EventDismissed != null ? s.EventDismissed.Value : nullDate,
                            NotificationSentTo = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].NotificationSentTo) : ""
                        }).OrderByDescending(x => x.EventDetected).ToList();

                    Response = Common.CreateResponse(result);
                }
                else
                {
                    var resultList = eventsdetectedRepository.All().Where(x => x.EventDetected.HasValue && x.EventDetected.Value.Month == statdate.Month && x.EventDetected.Value.Year == statdate.Year).ToList();
                    var result = resultList
                        .Select(s => new AlertsModel
                        {
                            DeviceName = (s.Device != "" && s.Device != null) ? s.Device : "System message",
                            DeviceType = s.DeviceType,
                            AlertType = s.EventType,
                            Details = s.Details,
                            EventDetected = s.EventDetected != null ? s.EventDetected.Value : nullDate,
                            EventDetectedSent = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].EventDetectedSent.Value) : nullDate,
                            EventDismissed = s.EventDismissed != null ? s.EventDismissed.Value : nullDate,
                            NotificationSentTo = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].NotificationSentTo) : ""
                        }).OrderByDescending(x => x.EventDetected).ToList();

                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }
        #endregion

        #region Notifications
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        private List<dynamic> GetNotificationsList(string deviceTypes = "")
        {
            List<dynamic> result_disp = new List<dynamic>();
            List<dynamic> result_sendto = new List<dynamic>();
            List<dynamic> result_escalateto = new List<dynamic>();
            List<dynamic> result_events = new List<dynamic>();
            List<dynamic> result_servers = new List<dynamic>();
            List<ServerObjects> result_servers_obj = new List<ServerObjects>();
            List<dynamic> result = new List<dynamic>();
            List<string> send_to = new List<string>();
            List<string> send_via = new List<string>();
            List<bool> is_selected_send = new List<bool>();
            List<string> event_ids = new List<string>();
            List<bool> is_selected_event = new List<bool>();
            List<string> server_ids = new List<string>();
            List<bool> is_selected_server = new List<bool>();
            List<string> hour_ids = new List<string>();
            List<bool> is_selected_hour = new List<bool>();
            FilterDefinition<BusinessHours> filterDef;

            try
            {
                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                notificationsRepository = new Repository<Notifications>(ConnectionString);
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                string[] deviceTypesArr;
                if (deviceTypes == "")
                    deviceTypes = "Domino,Sametime,URL,WebSphere,IBM Connections,Office365,Notes Database Replica,Notes Database";
                try
                {
                    deviceTypesArr = deviceTypes.Split(',');
                }
                catch (Exception ex)
                {
                    deviceTypesArr = new string[0];
                }

                //makes all notification destinations (and escalations)
                var notificationDestinations = notificationDestRepository.Collection.AsQueryable().ToList();
                var hoursname = "";
                foreach (var notificationDest in notificationDestinations)
                {
                    var is_sel = false;
                    if (notificationDest.Interval != null)
                    {
                        result_escalateto.Add(new HourDefinition
                        {
                            Id = notificationDest.Id.ToString(),
                            IsSelectedHour = false,
                            Interval = notificationDest.Interval,
                            SendVia = notificationDest.SendVia,
                            SendTo = notificationDest.SendTo
                        });
                    }
                    else
                    {
                        filterDef = businessHoursRepository.Filter.Eq(x => x.Id, notificationDest.BusinessHoursId);
                        var hourstype = businessHoursRepository.Find(filterDef).ToList();
                        if (hourstype.Count > 0)
                        {
                            hoursname = hourstype[0].Name;
                        }
                        result_sendto.Add(new HourDefinition
                        {
                            Id = notificationDest.Id.ToString(),
                            IsSelectedHour = false,
                            SendVia = notificationDest.SendVia,
                            SendTo = notificationDest.SendTo,
                            CopyTo = notificationDest.CopyTo == null ? "" : notificationDest.CopyTo,
                            BlindCopyTo = notificationDest.BlindCopyTo == null ? "" : notificationDest.BlindCopyTo,
                            BusinessHoursType = hoursname,
                            PersistentNotification = notificationDest.PersistentNotification
                        });
                    }
                }

                var notifications = notificationsRepository.Collection.AsQueryable().ToList();


                //Makes all events
                var eventsList = eventsMasterRepository.Find(_ => true).OrderBy(x => x.EventType).OrderBy(x => x.DeviceType).ToList();
                foreach (var eventItem in eventsList)
                {
                    result_events.Add(new EventDefinition
                    {
                        Id = eventItem.Id,
                        IsSelectedEvent = false,
                        EventType = eventItem.EventType,
                        DeviceType = eventItem.DeviceType,
                        NotificationOnRepeat = eventItem.NotificationOnRepeat
                    });
                    var is_sel = false;
                    is_selected_event.Add(is_sel);
                    event_ids.Add(eventItem.Id);
                }

                //makes all servers
                var locations = locationRepository.Find(x => true).ToList();
                var serversList = serversRepository.Find(serversRepository.Filter.In(x => x.DeviceType, deviceTypesArr)).OrderBy(x => x.DeviceName).ToList();
                foreach (var serverItem in serversList)
                {
                    result_servers_obj.Add(new ServerObjects()
                    {
                        DeviceId = serverItem.Id,
                        IsSelected = false,
                        CollectionName = "server",
                        DeviceName = serverItem.DeviceName,
                        DeviceType = serverItem.DeviceType,
                        LocationName = locations.Where(x => x.Id == serverItem.LocationId).Count() > 0 ? locations.Where(x => x.Id == serverItem.LocationId).First().LocationName : "N/A"
                    });
                }

                var serversOtherList = serverOtherRepository.Find(serverOtherRepository.Filter.In(x => x.Type, deviceTypesArr)).OrderBy(x => x.Name).ToList();
                foreach (var serverItem in serversOtherList)
                {
                    result_servers_obj.Add(new ServerObjects
                    {
                        DeviceId = serverItem.Id,
                        IsSelected = false,
                        CollectionName = "server_other",
                        DeviceName = serverItem.Name,
                        DeviceType = serverItem.Type,
                        LocationName = "N/A"
                    });
                }

                foreach (var notification in notifications)
                {
                    is_selected_event = new List<bool>();
                    event_ids = new List<string>();
                    event_ids = eventsList.Where(x => x.NotificationList != null && x.NotificationList.Contains(notification.Id)).Select(x => x.Id).ToList();

                    is_selected_server = new List<bool>();
                    var server_objects = new List<ServerObjects>();
                    server_objects = serversList.Where(x => x.NotificationList != null && x.NotificationList.Contains(notification.Id))
                        .Select(x => new ServerObjects() { DeviceId = x.Id, CollectionName = "server" }).ToList();
                    server_objects.AddRange(serversOtherList.Where(x => x.NotificationList != null && x.NotificationList.Contains(notification.Id))
                        .Select(x => new ServerObjects() { DeviceId = x.Id, CollectionName = "server_other" }).ToList());

                    var selectedHorus = notificationDestinations.Where(x => notification.SendList != null && notification.SendList.Contains(x.Id)).Select(x => x.Id).ToList();
                    

                    result_disp.Add(new NotificationsModel
                    {
                        ID = notification.Id.ToString(),
                        NotificationName = notification.NotificationName,
                        BusinessHoursIds = hour_ids,
                        EventIds = event_ids,
                        SelectedHours = selectedHorus,
                        ServerObjects = server_objects
                    });
                }

                

                result.Add(result_disp);
                result.Add(result_sendto);
                result.Add(result_escalateto);
                result.Add(result_events);
                result.Add(result_servers_obj.OrderBy(x => x.LocationName).ThenBy(x => x.DeviceName));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("notifications_list")]
        public APIResponse GetNotifications()
        {
            List<dynamic> result = new List<dynamic>();

            try
            {
                result = GetNotificationsList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }

        [HttpGet("notifications_selector")]
        public APIResponse GetNotificationsForSelect()
        {
            try
            {
                notificationsRepository = new Repository<Notifications>(ConnectionString);
                var result = notificationsRepository.All().Select(x => new NameValueModel
                {
                    Name = x.NotificationName,
                    Id = x.Id
                }).ToList();
                result.Insert(0, new NameValueModel { Name = "All", Id = "" });
                Response = Common.CreateResponse(new { notificationsList = result });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="isCombo"></param>
        /// <returns></returns>
        [HttpGet("get_alert_urls")]
        public APIResponse GetAlertURLs(bool isCombo = false)
        {
            try
            {
                if (!isCombo)
                {
                    alertURLsRepository = new Repository<Alert_URLs>(ConnectionString);
                    var result = alertURLsRepository.Collection.AsQueryable()
                        .Select(x => new AlertUrlDefinition
                        {
                            Id = x.Id,
                            Name = x.Name,
                            URL = x.Url
                        }
                        ).OrderBy(x => x.Name).ToList();
                    Response = Common.CreateResponse(result);
                }
                else
                {
                    alertURLsRepository = new Repository<Alert_URLs>(ConnectionString);
                    var result = alertURLsRepository.Collection.AsQueryable()
                        .Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Id }).OrderBy(x => x.DisplayText).ToList();
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }


        [HttpGet("get_scripts")]
        public APIResponse GetScripts(bool isCombo = false)
        {
            try
            {
                if (!isCombo)
                {
                    scriptsRepository = new Repository<Scripts>(ConnectionString);
                    var result = scriptsRepository.Collection.AsQueryable()
                        .Select(x => new ScriptDefinition
                        {
                            Id = x.Id,
                            ScriptName = x.ScriptName,
                            ScriptCommand = x.ScriptCommand,
                            ScriptLocation = x.ScriptLocation
                        }
                        ).OrderBy(x => x.ScriptName).ToList();
                    Response = Common.CreateResponse(result);
                }

                else
                {
                    scriptsRepository = new Repository<Scripts>(ConnectionString);
                    var result = scriptsRepository.Collection.AsQueryable()
                        .Select(x => new ComboBoxListItem { DisplayText = x.ScriptName, Value = x.Id }).OrderBy(x => x.DisplayText).ToList();
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="notificationDefinition"></param>
        /// <returns></returns>
        [HttpPut("save_hours_destinations")]
        public APIResponse UpdateHoursDestinations([FromBody]NotificationsModel notificationDefinition)
        {
            FilterDefinition<NotificationDestinations> filterDef;
            UpdateDefinition<NotificationDestinations> updateHours;
            FilterDefinition<BusinessHours> filterDefBusHrs;
            List<dynamic> result_sendto = new List<dynamic>();
            NotificationDestinations hoursdata;
            bool result = false;
            try
            {
                NotificationsModel notificationDef = notificationDefinition;
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                //need to get the id of the business hours definition based on the selected hours
                filterDefBusHrs = businessHoursRepository.Filter.Eq(x => x.Name, notificationDef.BusinessHoursType);
                var bushrs = businessHoursRepository.Find(filterDefBusHrs).ToList();
                var bushrsid = "";
                if (bushrs != null)
                {
                    bushrsid = bushrs[0].Id;
                }
                if (string.IsNullOrEmpty(notificationDefinition.ID))
                {
                    if (notificationDef.SendVia == "E-mail")
                    {
                        hoursdata = new NotificationDestinations
                        {
                            BusinessHoursId = bushrsid,
                            SendVia = notificationDefinition.SendVia,
                            SendTo = notificationDefinition.SendTo,
                            CopyTo = notificationDefinition.CopyTo,
                            BlindCopyTo = notificationDefinition.BlindCopyTo,
                            PersistentNotification = notificationDefinition.PersistentNotification
                        };
                    }
                    else if (notificationDef.SendVia == "SMS")
                    {
                        hoursdata = new NotificationDestinations
                        {
                            BusinessHoursId = bushrsid,
                            SendVia = notificationDefinition.SendVia,
                            SendTo = notificationDefinition.SendTo
                        };
                    }
                    else if (notificationDef.SendVia == "Script") 
                    {
                        hoursdata = new NotificationDestinations
                        {
                            BusinessHoursId = bushrsid,
                            SendVia = notificationDefinition.SendVia,
                            SendTo = notificationDefinition.SendTo
                        };
                    }
                    else if (notificationDef.SendVia == "URL")
                    {
                        hoursdata = new NotificationDestinations
                        {
                            BusinessHoursId = bushrsid,
                            SendVia = notificationDefinition.SendVia,
                            SendTo = notificationDefinition.SendTo
                        };
                    }
                    else
                    {
                        hoursdata = new NotificationDestinations
                        {
                            BusinessHoursId = bushrsid,
                            SendVia = notificationDefinition.SendVia
                        };
                    }
                    hoursdata.Id = notificationDestRepository.Insert(hoursdata);
                    var notificationDestinations = notificationDestRepository.Collection.AsQueryable().ToList();
                    foreach (var notificationDest in notificationDestinations)
                    {
                        var hoursname = "";
                        if (notificationDest.Interval == null)
                        {
                            filterDefBusHrs = businessHoursRepository.Filter.Eq(x => x.Id, notificationDest.BusinessHoursId);
                            var hourstype = businessHoursRepository.Find(filterDefBusHrs).ToList();
                            if (hourstype.Count > 0)
                            {
                                hoursname = hourstype[0].Name;
                            }
                            result_sendto.Add(new HourDefinition
                            {
                                Id = notificationDest.Id.ToString(),
                                SendVia = notificationDest.SendVia,
                                SendTo = notificationDest.SendTo,
                                CopyTo = notificationDest.CopyTo == null ? "" : notificationDest.CopyTo,
                                BlindCopyTo = notificationDest.BlindCopyTo == null ? "" : notificationDest.BlindCopyTo,
                                BusinessHoursType = hoursname,
                                PersistentNotification = notificationDest.PersistentNotification
                            });
                        }
                    }
                    Response = Common.CreateResponse(result_sendto, Common.ResponseStatus.Success.ToDescription(), "Hours and destinations inserted successfully");
                }
                else
                {
                    filterDef = notificationDestRepository.Filter.Eq(x => x.Id, notificationDef.ID);

                    if (notificationDef.SendVia != "E-mail")
                    {
                        updateHours = notificationDestRepository.Updater.Set(x => x.BusinessHoursId, bushrsid)
                        .Set(x => x.SendVia, notificationDef.SendVia)
                        .Set(x => x.SendTo, notificationDef.SendTo);
                    }
                    else
                    {
                        updateHours = notificationDestRepository.Updater.Set(x => x.BusinessHoursId, bushrsid)
                        .Set(x => x.SendVia, notificationDef.SendVia)
                        .Set(x => x.SendTo, notificationDef.SendTo)
                        .Set(x => x.CopyTo, notificationDef.CopyTo)
                        .Set(x => x.BlindCopyTo, notificationDef.BlindCopyTo)
                        .Set(x => x.PersistentNotification, notificationDef.PersistentNotification);
                    }
                    result = notificationDestRepository.Update(filterDef, updateHours);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Hours and destinations updated successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Updating hours and destinations has failed.\n Error Message :" + ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="notificationDefinition"></param>
        /// <returns></returns>
        [HttpPut("save_escalation")]
        public APIResponse UpdateEscalation([FromBody]NotificationsModel notificationDefinition)
        {
            FilterDefinition<NotificationDestinations> filterDef;
            UpdateDefinition<NotificationDestinations> updateHours;
            List<dynamic> result_sendto = new List<dynamic>();
            bool result = false;
            try
            {
                NotificationsModel notificationDef = notificationDefinition;
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                if (string.IsNullOrEmpty(notificationDefinition.ID))
                {
                    NotificationDestinations hoursdata = new NotificationDestinations
                    {
                        SendVia = notificationDefinition.SendVia,
                        SendTo = notificationDefinition.SendTo,
                        Interval = notificationDefinition.Interval
                    };
                    hoursdata.Id = notificationDestRepository.Insert(hoursdata);
                    var notificationDestinations = notificationDestRepository.Collection.AsQueryable().ToList();
                    foreach (var notificationDest in notificationDestinations)
                    {
                        if (notificationDest.Interval != null)
                        {
                            result_sendto.Add(new NotificationsModel
                            {
                                EscalationId = notificationDest.Id.ToString(),
                                SendVia = notificationDest.SendVia,
                                SendTo = notificationDest.SendTo,
                                Interval = notificationDest.Interval
                            });
                        }
                    }
                    Response = Common.CreateResponse(result_sendto, Common.ResponseStatus.Success.ToDescription(), "Escalation inserted successfully");
                }
                else
                {
                    filterDef = notificationDestRepository.Filter.Eq(x => x.Id, notificationDef.ID);

                    if (notificationDef.SendVia != "Script")
                    {
                        updateHours = notificationDestRepository.Updater.Set(x => x.SendVia, notificationDef.SendVia)
                        .Set(x => x.SendTo, notificationDef.SendTo)
                        .Set(x => x.Interval, notificationDef.Interval);
                    }
                    else
                    {
                        //adddscript update
                        updateHours = notificationDestRepository.Updater.Set(x => x.SendVia, notificationDef.SendVia)
                        .Set(x => x.SendTo, notificationDef.SendTo)
                        .Set(x => x.Interval, notificationDef.Interval);
                    }
                    result = notificationDestRepository.Update(filterDef, updateHours);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Escalation updated successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Updating escalation has failed.\n Error Message :" + ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="scriptDefinition"></param>
        /// <returns></returns>
        [HttpPut("save_script")]
        public APIResponse UpdateScript([FromBody]ScriptDefinition scriptDefinition)
        {
            FilterDefinition<Scripts> filterDef;
            List<dynamic> result_scripts = new List<dynamic>();
            UpdateDefinition<Scripts> updateScripts;
            bool result = false;
            try
            {
                scriptsRepository = new Repository<Scripts>(ConnectionString);
                if (string.IsNullOrEmpty(scriptDefinition.Id))
                {
                    Scripts scriptdata = new Scripts
                    {
                        ScriptName = scriptDefinition.ScriptName,
                        ScriptCommand = scriptDefinition.ScriptCommand,
                        ScriptLocation = scriptDefinition.ScriptLocation
                    };
                    scriptdata.Id = scriptsRepository.Insert(scriptdata);
                    var scriptList = scriptsRepository.Collection.AsQueryable().ToList();
                    foreach (var scriptval in scriptList)
                    {
                        result_scripts.Add(new ScriptDefinition
                        {
                            Id = scriptval.Id.ToString(),
                            ScriptName = scriptval.ScriptName,
                            ScriptCommand = scriptval.ScriptCommand,
                            ScriptLocation = scriptval.ScriptLocation
                        });
                    }
                    Response = Common.CreateResponse(result_scripts, Common.ResponseStatus.Success.ToDescription(), "Script inserted successfully");
                }
                else
                {
                    filterDef = scriptsRepository.Filter.Eq(x => x.Id, scriptDefinition.Id);
                    updateScripts = scriptsRepository.Updater.Set(x => x.ScriptName, scriptDefinition.ScriptName)
                       .Set(x => x.ScriptCommand, scriptDefinition.ScriptCommand)
                       .Set(x => x.ScriptLocation, scriptDefinition.ScriptLocation);
                    result = scriptsRepository.Update(filterDef, updateScripts);
                    Response = Common.CreateResponse(scriptDefinition.Id, Common.ResponseStatus.Success.ToDescription(), "Script updated successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

        [HttpPut("save_alert_url")]
        public APIResponse UpdateAlertUrl([FromBody]AlertUrlDefinition alert_url_definition)
        {
            FilterDefinition<Alert_URLs> filterDef;
            List<dynamic> result_urls = new List<dynamic>();
            UpdateDefinition<Alert_URLs> updateAlertUrls;
            bool result = false;
            try
            {
                alertURLsRepository = new Repository<Alert_URLs>(ConnectionString);
                if (string.IsNullOrEmpty(alert_url_definition.Id))
                {
                    Alert_URLs alertUrlsData = new Alert_URLs()
                    {
                        Name = alert_url_definition.Name,
                        Url  = alert_url_definition.URL
                    };
                    alertUrlsData.Id = alertURLsRepository.Insert(alertUrlsData);
                    var urlsList = alertURLsRepository.Collection.AsQueryable().ToList();
                    foreach (var urlVal in urlsList)
                    {
                        result_urls.Add(new AlertUrlDefinition
                        {
                            Id = urlVal.Id.ToString(),
                            Name = urlVal.Name,
                            URL = urlVal.Url,
                          
                        });
                    }
                    Response = Common.CreateResponse(result_urls, Common.ResponseStatus.Success.ToDescription(), "Alert URL inserted successfully");
                }
                else
                {
                    filterDef = alertURLsRepository.Filter.Eq(x => x.Id, alert_url_definition.Id);
                    updateAlertUrls = alertURLsRepository.Updater.Set(x => x.Name, alert_url_definition.Name)
                       .Set(x => x.Url, alert_url_definition.URL);
                       
                    result = alertURLsRepository.Update(filterDef, updateAlertUrls);
                    Response = Common.CreateResponse(alert_url_definition.Id, Common.ResponseStatus.Success.ToDescription(), "Alert URL updated successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="notificationDefinition"></param>
        /// <returns></returns>
        [HttpPut("save_notification_definition")]
        public APIResponse UpdateNotificationDefinition([FromBody]NotificationsModel notificationDefinition)
        {
            FilterDefinition<Notifications> filterNotificationDef;
            UpdateDefinition<Notifications> updateNotificationDef;
            FilterDefinition<Server> filterServerDef;
            FilterDefinition<EventsMaster> filterEventsDef;
            List<string> notificationData = new List<string>();
            List<string> serversData = new List<string>();
            List<string> eventsData = new List<string>();
            List<string> tempList = new List<string>();
            List<dynamic> saveresult = new List<dynamic>();
            string _id = "";
            try
            {
                notificationsRepository = new Repository<Notifications>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                
                // Create a new notification definition
                if (string.IsNullOrWhiteSpace(notificationDefinition.ID))
                {
                    // 1. Create a notifications document with a list of hours and destinations and escalation id's in SendList

                    Notifications notification = new Notifications();
                    notification.Id = ObjectId.GenerateNewId().ToString();
                    _id = notification.Id;
                    notification.NotificationName = notificationDefinition.NotificationName;
                    notification.SendList = notificationDefinition.SelectedHours;
                    notificationsRepository.Insert(notification);

                    // 2. Update the server documents with a list of notifications - add a notification to NotificationList

                    filterServerDef = serversRepository.Filter.In(x => x.Id, notificationDefinition.ServerObjects.Where(x => x.CollectionName == "server").Select(x => x.DeviceId));
                    var updateServerDef = serversRepository.Updater.AddToSet(x => x.NotificationList, notification.Id);
                    serversRepository.Update(filterServerDef, updateServerDef);

                    var filterServerOtherDef = serverOtherRepository.Filter.In(x => x.Id, notificationDefinition.ServerObjects.Where(x => x.CollectionName == "server_other").Select(x => x.DeviceId));
                    var updateServerOtherDef = serverOtherRepository.Updater.AddToSet(x => x.NotificationList, notification.Id);
                    serversRepository.Update(filterServerDef, updateServerDef);


                    // 3. Update the events_master documents with a list of notifications - add a notificaion to NotificationList

                    filterEventsDef = eventsMasterRepository.Filter.In(x => x.Id, notificationDefinition.EventIds);
                    var updateEventsDef = eventsMasterRepository.Updater.AddToSet(x => x.NotificationList, notification.Id);
                    eventsMasterRepository.Update(filterEventsDef, updateEventsDef);
                    
                    saveresult = GetNotificationsList();
                    Response = Common.CreateResponse(saveresult, Common.ResponseStatus.Success.ToDescription(), "Notification definition inserted successfully");
                }
                // Update an existing notification definition
                else
                {
                    // 1. Update the notifications document with a list of hours and destinations and escalation id's

                    filterNotificationDef = notificationsRepository.Filter.Eq(x => x.Id, notificationDefinition.ID);
                    updateNotificationDef = notificationsRepository.Updater.Set(x => x.SendList, notificationDefinition.SelectedHours).Set(x => x.NotificationName, notificationDefinition.NotificationName);
                    var result = notificationsRepository.Update(filterNotificationDef, updateNotificationDef);

                    // 2. Update the server documents with a list of notifications. The algorithm is as follows:
                    //    2.1. Get server documents that have a NotificationList embedded document containing the notification id
                    //    2.2. Parse the documents and update the NotificationList of the ones that are no longer associated with the notification
                    //    2.3. Get server documents that match the id's listed in the notification - serversData
                    //    2.4. Update the NotificationList of the documents by adding the current notification id to NotificationList


                    filterServerDef = serversRepository.Filter.Where(x => true);
                    var updateServerDef = serversRepository.Updater.Pull(x => x.NotificationList, notificationDefinition.ID);
                    serversRepository.Update(filterServerDef, updateServerDef);

                    filterServerDef = serversRepository.Filter.In(x => x.Id, notificationDefinition.ServerObjects.Where(x => x.CollectionName == "server").Select(x => x.DeviceId));
                    updateServerDef = serversRepository.Updater.AddToSet(x => x.NotificationList, notificationDefinition.ID);
                    serversRepository.Update(filterServerDef, updateServerDef);

                    var filterServerOtherDef = serverOtherRepository.Filter.Where(x => true);
                    var updateServerOtherDef = serverOtherRepository.Updater.Pull(x => x.NotificationList, notificationDefinition.ID);

                    filterServerOtherDef = serverOtherRepository.Filter.In(x => x.Id, notificationDefinition.ServerObjects.Where(x => x.CollectionName == "server_other").Select(x => x.DeviceId));
                    updateServerOtherDef = serverOtherRepository.Updater.AddToSet(x => x.NotificationList, notificationDefinition.ID);
                    serverOtherRepository.Update(filterServerOtherDef, updateServerOtherDef);


                    // 3. Update the events_master documents with a list of notifications. The algorithm is as follows:
                    //    2.1. Get events_master documents that have a NotificationList embedded document containing the notification id
                    //    2.2. Parse the documents and update the NotificationList of the ones that are no longer associated with the notification
                    //    2.3. Get events_master documents that match the id's listed in the notification - eventsData
                    //    2.4. Update the NotificationList of the documents by adding the current notification id to NotificationList

                    filterEventsDef = eventsMasterRepository.Filter.Where(x => true);
                    var updateEventsDef = eventsMasterRepository.Updater.Pull(x => x.NotificationList, notificationDefinition.ID);
                    eventsMasterRepository.Update(filterEventsDef, updateEventsDef);

                    filterEventsDef = eventsMasterRepository.Filter.In(x => x.Id, notificationDefinition.EventIds);
                    updateEventsDef = eventsMasterRepository.Updater.AddToSet(x => x.NotificationList, notificationDefinition.ID);
                    eventsMasterRepository.Update(filterEventsDef, updateEventsDef);
                    
                    saveresult = GetNotificationsList();
                    Response = Common.CreateResponse(saveresult, Common.ResponseStatus.Success.ToDescription(), "Notification definition updated successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notification definition update has failed. " + ex.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        [HttpDelete("delete_hours_destinations/{id}")]
        public APIResponse DeleteHoursDestinations(string id)
        {
            try
            {
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                Expression<Func<NotificationDestinations, bool>> expression = (p => p.Id == id);
                notificationDestRepository.Delete(expression);
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Hours and destinations deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of hours and destinations has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        [HttpDelete("delete_script/{id}")]
        public APIResponse DeleteScript(string id)
        {
            try
            {
                scriptsRepository = new Repository<Scripts>(ConnectionString);
                Expression<Func<Scripts, bool>> expression = (p => p.Id == id);
                scriptsRepository.Delete(expression);
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Script deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of script has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpDelete("delete_alert_url/{id}")]
        public APIResponse DeleteAlertUrl(string id)
        {
            try
            {
                alertURLsRepository = new Repository<Alert_URLs>(ConnectionString);
                Expression<Func<Alert_URLs, bool>> expression = (p => p.Id == id);
                alertURLsRepository.Delete(expression);
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "URL deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of script has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        [HttpDelete("delete_notification_definition/{id}")]
        public APIResponse DeleteNotificationDefinition(string id)
        {
            FilterDefinition<Server> filterServerDef;
            FilterDefinition<EventsMaster> filterEventsDef;
            try
            {
                // 1. Delete the notification document from the notifications collection

                notificationsRepository = new Repository<Notifications>(ConnectionString);
                Expression<Func<Notifications, bool>> expression = (p => p.Id == id);
                notificationsRepository.Delete(expression);

                // 2. Update the events_master collection - remove id from the notifications embedded document

                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                filterEventsDef = eventsMasterRepository.Filter.AnyEq(x => x.NotificationList, id);
                var eventsList = eventsMasterRepository.Find(filterEventsDef).ToList();

                if (eventsList.Count > 0)
                {
                    foreach (var eventDef in eventsList)
                    {
                        var notificationList = eventDef.NotificationList.ToList();
                        var itemToRemove = notificationList.Single(r => r == id);
                        eventDef.NotificationList.Remove(itemToRemove);
                        eventsMasterRepository.Replace(eventDef);
                    }
                }

                // 3. Update the server collection - remove id from the notifications embedded document

                serversRepository = new Repository<Server>(ConnectionString);
                filterServerDef = serversRepository.Filter.AnyEq(x => x.NotificationList, id);
                var serversList = serversRepository.Find(filterServerDef).ToList();

                if (serversList.Count > 0)
                {
                    foreach (var serverDef in serversList)
                    {
                        var notificationList = serverDef.NotificationList.ToList();
                        var itemToRemove = notificationList.Single(r => r == id);
                        serverDef.NotificationList.Remove(itemToRemove);
                        serversRepository.Replace(serverDef);
                    }
                }
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Notification definition deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Deletion of notification has failed.\n Error Message :" + exception.Message);
            }
            return Response;

        }

        [HttpGet("notifications_by_id")]
        public APIResponse GetNotificationsById(string id = "")
        {
            List<string> events = new List<string>();
            List<string> devicetypes = new List<string>();
            List<string> servers = new List<string>();
            EventFilter eventfilter = new EventFilter();
            List<EventFilter> eventfilters = new List<EventFilter>();
            List<Notifications> notifications = new List<Notifications>();
            FilterDefinition<EventsMaster> filterEventsDef;
            FilterDefinition<Server> filterServerDef;
            FilterDefinition<EventsDetected> filterEventsDetectedDef;
            DateTime? nullDate = null;

            try
            {
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                notificationsRepository = new Repository<Notifications>(ConnectionString);
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                if (!string.IsNullOrEmpty(id))
                {
                    notifications = notificationsRepository.Collection.AsQueryable().Where(i => i.Id == id).ToList();
                    if (notifications.Count > 0)
                    {
                        //Get a list of all possible recipients
                        foreach (var notification in notifications)
                        {
                            eventfilter = new EventFilter();
                            //Get a list of all events that get specified notifications
                            filterEventsDef = eventsMasterRepository.Filter.AnyEq(x => x.NotificationList, notification.Id);
                            var eventsList = eventsMasterRepository.Find(filterEventsDef).ToList();
                            if (eventsList.Count > 0)
                            {
                                foreach (var eventval in eventsList)
                                {
                                    events.Add(eventval.EventType);
                                    devicetypes.Add(eventval.DeviceType);
                                }
                                eventfilter.EventTypes = events;
                                eventfilter.DeviceTypes = devicetypes;
                            }
                            //Get a list of all devices that get specified notifications
                            filterServerDef = serversRepository.Filter.AnyEq(x => x.NotificationList, notification.Id);
                            var serversList = serversRepository.Find(filterServerDef).ToList();
                            if (serversList.Count > 0)
                            {
                                foreach (var serverval in serversList)
                                {
                                    servers.Add(serverval.DeviceName);
                                }
                                eventfilter.DeviceNames = servers;
                            }
                            eventfilters.Add(eventfilter);
                        }
                        foreach (var eventfilterval in eventfilters)
                        {
                            filterEventsDetectedDef = eventsdetectedRepository.Filter.And(eventsdetectedRepository.Filter.In(i => i.DeviceType, eventfilterval.DeviceTypes),
                                eventsdetectedRepository.Filter.In(i => i.Device, eventfilterval.DeviceNames),
                                eventsdetectedRepository.Filter.In(i => i.EventType, eventfilterval.EventTypes));
                            var eventsDetected = eventsdetectedRepository.Find(filterEventsDetectedDef).ToList();
                            if (eventsDetected.Count > 0)
                            {
                                var result = eventsDetected.Select(s => new AlertsModel
                                 {
                                     DeviceName = s.Device,
                                     DeviceType = s.DeviceType,
                                     AlertType = s.EventType,
                                     Details = s.Details,
                                     EventDetectedSent = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].EventDetectedSent.Value) : nullDate,
                                     EventDismissed = s.EventDismissed != null ? s.EventDismissed.Value : nullDate,
                                     NotificationSentTo = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].NotificationSentTo) : ""
                                 }).OrderBy(x => x.DeviceName).OrderByDescending(x => x.EventDetected).ToList();
                                Response = Common.CreateResponse(result);
                            }
                        }
                    }
                }
                else
                {
                    var eventsDetected = eventsdetectedRepository.Collection.AsQueryable().ToList();
                    if (eventsDetected.Count > 0)
                    {
                        var result = eventsDetected.Select(s => new AlertsModel
                        {
                            DeviceName = s.Device,
                            DeviceType = s.DeviceType,
                            AlertType = s.EventType,
                            Details = s.Details,
                            EventDetectedSent = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].EventDetectedSent.Value) : nullDate,
                            EventDismissed = s.EventDismissed != null ? s.EventDismissed.Value : nullDate,
                            NotificationSentTo = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count - 1].NotificationSentTo) : ""
                        }).OrderBy(x => x.DeviceName).OrderByDescending(x => x.EventDetected).ToList();
                        Response = Common.CreateResponse(result);
                    }
                }
                if (Response == null)
                {
                    Response = Common.CreateResponse(null, "", "");
                }            
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }
        #endregion

        [HttpPut("clear_alerts")]
        public APIResponse ClearAlerts()
        {
            UpdateDefinition<EventsDetected> updateDef;

            try
            {
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                var builder = Builders<EventsDetected>.Filter;
                //Find events for which notifications have gone out but which have not been cleared
                var filter = builder.Exists("notifications_sent", true) & builder.Exists("notifications_sent.event_dismissed_sent", false) & builder.Exists("event_dismissed", false);
                updateDef = eventsdetectedRepository.Updater
                        .Set(p => p.NotificationsSent[-1].EventDismissedSent, DateTime.UtcNow)
                        .Set(p => p.EventDismissed, DateTime.UtcNow);
                var res = eventsdetectedRepository.Update(filter, updateDef);
                //Find events for which notifications never went out
                filter = builder.Exists("notifications_sent", false) & builder.Exists("event_dismissed", false);
                updateDef = eventsdetectedRepository.Updater
                        .Set(p => p.EventDismissed, DateTime.UtcNow);
                res = eventsdetectedRepository.Update(filter, updateDef);
                Response = Common.CreateResponse(true, "Success", "Past events have been cleared");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Clearing events has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("delete_alerts")]
        public APIResponse DeleteAlerts()
        {
            FilterDefinition<EventsDetected> filterDef;
            
            try
            {
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                Expression<Func<EventsDetected, bool>> expression = (x => x.Id != null);
                filterDef = eventsdetectedRepository.Filter.Exists(x => x.Id, true);
                eventsdetectedRepository.Delete(expression);
                Response = Common.CreateResponse(true, "Success", "Past events have been deleted");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Deleting events has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Mail
        [HttpGet("notesmail_probes_list")]
        public APIResponse GetNotesMailProbes()
        {
            List<NotesMailProbesModel> result = new List<NotesMailProbesModel>();
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                result = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType == "NotesMail Probe")
                    .Select(x => new NotesMailProbesModel
                {
                    Id = x.Id,
                    Name = x.DeviceName,
                    IsEnabled = x.IsEnabled.HasValue && x.IsEnabled!=null ? x.IsEnabled : false,
                    Category = x.Category,
                    Threshold = x.DeliveryThreshold,
                    ScanInterval = x.ScanInterval,
                    OffHoursInterval = x.OffHoursScanInterval,
                    RetryInterval = x.RetryInterval,
                    SourceServer = x.SourceServer,
                    DestinationServer = x.TargetServer,
                    SendTo = x.SendToAddress,
                    EchoService = x.SendToEchoService,
                    ReplyTo = x.ReplyToAddress,
                    DestinationDatabase = x.TargetDatabase
                }).OrderBy(x => x.Name).ToList();
                foreach(var x in result)
                {
                    x.IsEnabled = x.IsEnabled.HasValue && x.IsEnabled != null ? x.IsEnabled : false;
                }
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting NotesMail probes has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_notesmail_probes")]
        public APIResponse UpdateNotesMailPobes([FromBody]NotesMailProbesModel notesMailProbe)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> filterExpression;
                if (string.IsNullOrEmpty(notesMailProbe.Id))
                {
                    filterExpression = (p => p.DeviceName == notesMailProbe.Name && p.DeviceType == "NotesMail Probe");
                }
                else
                {
                    filterExpression = (p => p.DeviceName == notesMailProbe.Name && p.Id != notesMailProbe.Id && p.DeviceType == "NotesMail Probe");
                }
                var existedData = serversRepository.Find(filterExpression).Select(x => x.DeviceName).FirstOrDefault();
                if (existedData == null)
                {
                    if (string.IsNullOrEmpty(notesMailProbe.Id))
                    {
                        Server notesMailProbes = new Server
                        {
                            DeviceName = notesMailProbe.Name,
                            DeviceType = "NotesMail Probe",
                            IsEnabled = notesMailProbe.IsEnabled,
                            Category = notesMailProbe.Category,
                            DeliveryThreshold = notesMailProbe.Threshold,
                            ScanInterval = notesMailProbe.ScanInterval,
                            OffHoursScanInterval = notesMailProbe.OffHoursInterval,
                            RetryInterval = notesMailProbe.RetryInterval,
                            SourceServer = notesMailProbe.SourceServer,
                            TargetServer = notesMailProbe.DestinationServer,
                            SendToAddress = notesMailProbe.SendTo,
                            SendToEchoService = notesMailProbe.EchoService,
                            ReplyToAddress = notesMailProbe.ReplyTo,
                            TargetDatabase = notesMailProbe.DestinationDatabase
                        };
                        string id = serversRepository.Insert(notesMailProbes);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "NotesMail Probe inserted successfully.");
                    }
                    else
                    {
                        FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == notesMailProbe.Id);
                        var updateDefination = serversRepository.Updater.Set(p => p.DeviceName, notesMailProbe.Name)
                            .Set(p => p.IsEnabled, notesMailProbe.IsEnabled)
                            .Set(p => p.Category, notesMailProbe.Category)
                            .Set(p => p.DeliveryThreshold, notesMailProbe.Threshold)
                            .Set(p => p.ScanInterval, notesMailProbe.ScanInterval)
                            .Set(p => p.OffHoursScanInterval, notesMailProbe.OffHoursInterval)
                            .Set(p => p.RetryInterval, notesMailProbe.RetryInterval)
                            .Set(p => p.SourceServer, notesMailProbe.SourceServer)
                            .Set(p => p.TargetServer, notesMailProbe.DestinationServer)
                            .Set(p => p.SendToAddress, notesMailProbe.SendTo)
                            .Set(p => p.SendToEchoService, notesMailProbe.EchoService)
                            .Set(p => p.ReplyToAddress, notesMailProbe.ReplyTo)
                            .Set(p => p.TargetDatabase, notesMailProbe.DestinationDatabase);
                        var result = serversRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "NotesMail Probe updated successfully.");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "A NotesMail probe with the same name already exists.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "NotesMail Probe update has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpDelete("notesmail_probe/{Id}")]
        public APIResponse DeleteNotesMailProbe(string Id)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == Id);
                serversRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "NotesMail Probe deleted sucessfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "NotesMail Probe deletion has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <author> </author>
        /// <returns></returns>
        [HttpGet("servers_list")]
        public APIResponse GetAllServersList()
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var result = serversRepository.All().Select(x => new ServersModel
                {

                    DeviceName = x.DeviceName,
                    DeviceType = x.DeviceType,
                    Description = x.Description,
                    DeviceId = x.Id


                }).ToList();


                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching Maintenance failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_device_type_list")]
        public APIResponse GetDeviceTypes()
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var result = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType != "URL" && x.DeviceType != "WebSphereCell" && x.DeviceType != "WebSphereNode").Select(x => new ComboBoxListItem { DisplayText = x.DeviceType, Value = x.DeviceType }).Distinct().ToList().OrderBy(x => x.DisplayText);


                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching maintenance has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("mobileusers")]
        public APIResponse GetAllMobileUsersList()
        {
            try
            {
                mobiledevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var result = mobiledevicesRepository.All().Select(x => new MobileUserDevice
                {
                    UserName = x.UserName,
                    DeviceName = x.DeviceName,
                    LastSyncTime = x.LastSyncTime



                }).ToList();


                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching mobile users has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>

        [HttpGet("device_list")]
        public APIResponse GetAllServersWithLocation()
        {
            credentialsRepository = new Repository<Credentials>(ConnectionString);
            serversRepository = new Repository<Server>(ConnectionString);
            serverOtherRepository = new Repository<ServerOther>(ConnectionString);
            locationRepository = new Repository<Location>(ConnectionString);
            List<ServerLocation> serverLocations = new List<ServerLocation>();
            Expression<Func<Credentials, bool>> expression;
            try
            {
                var locationList = locationRepository.Find(x => true).ToList();
                var credentialList = credentialsRepository.Find(x => true).ToList();
                var result = serversRepository.Find(x => true).ToList();
                foreach (var x in result)
                {
                    ServerLocation serverLocation = new ServerLocation()
                    {
                        Id = x.Id,
                        DeviceName = x.DeviceName,
                        DeviceType = x.DeviceType,
                        Description = x.Description,
                        AssignedNode = x.AssignedNode,
                        CurrentNode = x.CurrentNode,
                        LocationName = locationList.Where(y => y.Id == x.LocationId).Count() > 0 ? locationList.Where(y => y.Id == x.LocationId).First().LocationName : null,
                        IsSelected = false,
                        Category = x.Category == null ? "" : x.Category,
                        Credentials = credentialList.Where(y => y.Id == x.CredentialsId).Count() > 0 ? credentialList.Where(y => y.Id == x.CredentialsId).First().Alias : null
                    };
                    serverLocations.Add(serverLocation);
                }

                var result2 = serverOtherRepository.Find(x => true).ToList();
                foreach (var x in result2)
                {
                    ServerLocation serverLocation = new ServerLocation()
                    {
                        Id = x.Id,
                        DeviceName = x.Name,
                        DeviceType = x.Type,
                        Description = "",
                        AssignedNode = x.AssignedNode,
                        CurrentNode = x.CurrentNode,
                        LocationName = null,
                        IsSelected = false,
                        Category = x.Category == null ? "" : x.Category,
                        Credentials = null
                    };
                    serverLocations.Add(serverLocation);
                }
                Response = Common.CreateResponse(serverLocations.OrderBy(x => x.DeviceName));
                /*
                //var result = serversRepository.Collection.Aggregate()
                //                                         .Lookup("location", "location_id", "_id", "result").ToList();
                foreach (var x in result)
                {
                    ServerLocation serverLocation = new ServerLocation();
                    {
                        serverLocation.Id = x["_id"].AsObjectId.ToString();
                        serverLocation.DeviceName = x.GetValue("device_name", BsonString.Create(string.Empty)).ToString();
                        serverLocation.DeviceType = x.GetValue("device_type", BsonString.Create(string.Empty)).ToString();
                        serverLocation.Description = x.Contains("description") ? (x.GetValue("description").IsBsonNull ? "" : x.GetValue("description", BsonString.Create(string.Empty)).ToString()) : "";
                        serverLocation.AssignedNode = x.GetValue("assigned_node", BsonString.Create(string.Empty)).ToString();
                        serverLocation.CurrentNode = x.GetValue("current_node", BsonString.Create(string.Empty)).ToString();
                        if (x.GetValue("result", BsonValue.Create(string.Empty)).AsBsonArray.Values.Count() > 0)
                        {
                            serverLocation.LocationName = x.GetValue("result", BsonValue.Create(string.Empty))[0]["location_name"].ToString();
                        }
                        serverLocation.IsSelected = false;
                        serverLocation.Category = x.Contains("category") ? (x.GetValue("category").IsBsonNull ? "" : x.GetValue("category", BsonString.Create(string.Empty)).ToString()) : "";
                    }
                    var cred_id = x.Contains("credentials_id") ? (x.GetValue("credentials_id").IsBsonNull ? "" : x.GetValue("credentials_id", BsonString.Create(string.Empty)).ToString()) : "";
                    if (cred_id != "")
                    {
                        var creds = credentialsRepository.Collection.AsQueryable().Where(p => p.Id == cred_id).ToList();
                        if (creds.Count > 0)
                        {
                            serverLocation.Credentials = creds[0].Alias;
                        }
                    }
                    serverLocations.Add(serverLocation);
                }
                
                var result2 = serverOtherRepository.Collection.Aggregate()
                                                         .Lookup("location", "location_id", "_id", "result").ToList();
                foreach (var x in result2)
                {
                    ServerLocation serverLocation = new ServerLocation();
                    {
                        serverLocation.Id = x["_id"].AsObjectId.ToString();
                        serverLocation.DeviceName = x.GetValue("name", BsonString.Create(string.Empty)).ToString();
                        serverLocation.DeviceType = x.GetValue("type", BsonString.Create(string.Empty)).ToString();
                        serverLocation.Description = x.Contains("description") ? (x.GetValue("description").IsBsonNull ? "" : x.GetValue("description", BsonString.Create(string.Empty)).ToString()) : "";
                        serverLocation.AssignedNode = x.GetValue("assigned_node", BsonString.Create(string.Empty)).ToString();
                        serverLocation.CurrentNode = x.GetValue("current_node", BsonString.Create(string.Empty)).ToString();
                        if (x.GetValue("result", BsonValue.Create(string.Empty)).AsBsonArray.Values.Count() > 0)
                        {
                            serverLocation.LocationName = x.GetValue("result", BsonValue.Create(string.Empty))[0]["location_name"].ToString();
                        }
                        serverLocation.IsSelected = false;
                        var cred_id = x.Contains("credentials_id") ? (x.GetValue("credentials_id").IsBsonNull ? "" : x.GetValue("credentials_id", BsonString.Create(string.Empty)).ToString()) : "";
                        serverLocation.Category = x.Contains("category") ? (x.GetValue("category").IsBsonNull ? "" : x.GetValue("category", BsonString.Create(string.Empty)).ToString()) : "";
                        if (cred_id != "")
                        {
                            var creds = credentialsRepository.Collection.AsQueryable().Where(p => p.Id == cred_id).ToList();
                            if (creds.Count > 0)
                            {
                                serverLocation.Credentials = creds[0].Alias;
                            }
                        }
                    }
                    serverLocations.Add(serverLocation);
                }
                Response = Common.CreateResponse(serverLocations.OrderBy(x => x.DeviceName));
                */
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="windowsservicesettings"></param>
        /// <returns></returns>

        [HttpPut("save_windows_services")]
        public APIResponse SaveWindowsServices([FromBody]DeviceSettings windowsservicesettings)
        {
            try
            {
                var windowsServiceValues = ((Newtonsoft.Json.Linq.JArray)windowsservicesettings.Setting).ToObject<List<string>>();
                var devicesList = ((Newtonsoft.Json.Linq.JArray)windowsservicesettings.Devices).ToObject<string[]>();
                
                serversRepository = new Repository<Server>(ConnectionString);
                FilterDefinition<Server> filterDefinition = serversRepository.Filter.In(x => x.Id, devicesList);
                Server serverDoc = serversRepository.Find(filterDefinition).First();

                serverDoc.WindowServices.ForEach(x => x.Monitored = windowsServiceValues.Contains(x.ServiceName));

                UpdateDefinition<Server> updateDefinition = serversRepository.Updater.Set(x => x.WindowServices, serverDoc.WindowServices);
                serversRepository.Update(filterDefinition, updateDefinition);

                Response = Common.CreateResponse("Success", "Ok", "Window services have successfully been updated.");

                //if (devicesList.Count() > 0 && windowsServiceValues.Count() > 0)
                //{

                //    foreach (string id in devicesList)
                //    {
                //        var server = serversRepository.Get(id);
                //        List<WindowsService> windowsServices = new List<WindowsService>();


                //        foreach (var serverTask in windowsServiceValues)
                //        {
                //            WindowsService windowsService = new WindowsService();
                //            windowsService.ServiceName = serverTask.TaskName;

                //            windowsServices.Add(windowsService);
                //        }
                //        windowsServices.AddRange(server.ServerTasks);

                //        if (windowsServices.Count > 0)
                //        {
                //            updateDefinition = serversRepository.Updater.Set(p => p.ServerTasks, windowsServices);
                //            var result = serversRepository.Update(server, updateDefinition);
                //        }

                //    }
                //    Response = Common.CreateResponse(null, "OK", "Settings are not selected");


                //}
                //else
                //{
                //    Response = Common.CreateResponse(null, "Error", "Devices were not selected");
                //}

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving Windows Services has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="device_id"></param>
        /// <returns></returns>
        [HttpGet("{device_id}/get_maintenance_windows")]
        public APIResponse GetMaintenanceWindows(string device_id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {

                Expression<Func<Server, bool>> expression = (p => p.Id == device_id);
                var result = serversRepository.Find(expression).Select(x => new Maintenance
                {
                    //DeviceId = x.DeviceId,
                    //Type = x.Type,
                    //Category = x.category,
                    //LastScan = Convert.ToString(x.LastUpdate.Value),
                    //TestName = x.TestName,
                    //Result = x.Result,
                    //Details = x.Details
                });
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }

        [HttpPut("Send_Smtpservers")]
        public APIResponse SmtpSevers([FromBody]EmailServer emailid )
        {
            try
            {
                Common common = new Common();
                string Body = "Vitalsigns test";
                string Subject = "test smtp server1";
                string decreptedpassword = "";
                if (emailid.emailtype == "primary")
                {
                    VSFramework.RegistryHandler registery = new VSFramework.RegistryHandler();
                    string encryptedpaswword = Convert.ToString(registery.ReadFromRegistry("Primarypwd"));
                    VSFramework.TripleDES tripledes = new VSFramework.TripleDES();
                    decreptedpassword = tripleDes.Decrypt(encryptedpaswword);

                }
                else if (emailid.emailtype == "Secondary")
                {
                    VSFramework.RegistryHandler registery = new VSFramework.RegistryHandler();
                    string encryptedpaswword = Convert.ToString(registery.ReadFromRegistry("secondarypwd"));
                    VSFramework.TripleDES tripledes = new VSFramework.TripleDES();
                    decreptedpassword = tripleDes.Decrypt(encryptedpaswword);
                }

                common.SendSmtpSevers(emailid.emailId,emailid.password,emailid.emailHostName,emailid.emailUserId,decreptedpassword, emailid.emailPort,emailid.emailSSL == "", Body, Subject);

           
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Email Send  successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Email Send  has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #region  Simulation Tests
        /// <summary>
        /// 
        /// </summary>
        /// <author>Sowmya</author>
        /// <param name="ibmsimulations"></param>
        /// <returns></returns>
        [HttpPut("save_simulationtests")]
        public APIResponse SaveSimulation([FromBody]SimulationModel ibmsimulations)
        {
            List<NameValuePair> nameValuePairs = new List<NameValuePair>();
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                if (ibmsimulations.CreateActivity)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Activity Threshold", Value = Convert.ToString(ibmsimulations.CreateActivityThreshold) });
                if (ibmsimulations.CreateBlog)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Blog Threshold", Value = Convert.ToString(ibmsimulations.CreateBlogThreshold) });
                if (ibmsimulations.CreateBookmark)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Bookmark Threshold", Value = Convert.ToString(ibmsimulations.CreateBookmarkThreshold) });
                if (ibmsimulations.CreateCommunity)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Community Threshold", Value = Convert.ToString(ibmsimulations.CreateCommunityThreshold) });
                if (ibmsimulations.CreateFile)
                    nameValuePairs.Add(new NameValuePair { Name = "Create File Threshold", Value = Convert.ToString(ibmsimulations.CreateFileThreshold) });
                if (ibmsimulations.CreateWiki)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Wiki Threshold", Value = Convert.ToString(ibmsimulations.CreateWikiThreshold) });
                if (ibmsimulations.SearchProfile)
                    nameValuePairs.Add(new NameValuePair { Name = "Search Profile Threshold", Value = Convert.ToString(ibmsimulations.SearchProfileThreshold) });

                Server server = serversRepository.Get(ibmsimulations.Id);
                var updateDefination = serversRepository.Updater
                    .Set(p => p.SimulationTests, nameValuePairs)
                    .Set(p => p.ConnectionsCommunityUuid, ibmsimulations.CommunityUUID)
                    .Set(p => p.ConnectionsTestUrl, ibmsimulations.TestUrl);
                var result = serversRepository.Update(server, updateDefination);
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Simulation tests updated successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving simulation tests has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpGet("{device_id}/get_simulationtests")]
        public APIResponse GetSimulation(string device_id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                var simulationSettings = new List<string> { "Create Activity Threshold", "Create Activity", "Create Blog Threshold", "Create Blog", "Create Bookmark Threshold", "Create Bookmark", "Create Community Threshold", "Create Community", "Create File Threshold", "Create File", "Create Wiki Threshold", "Create Wiki", "Search Profile Threshold", "Search Profile" };
                SimulationModel ibmsimulations = new SimulationModel();
                ibmsimulations.Id = device_id;
                var result = serversRepository.Collection.AsQueryable().Where(x => x.Id == device_id).FirstOrDefault();
                if (result.SimulationTests != null)
                {
                    if (result.SimulationTests.Where(x => x.Name == "Create Activity Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateActivity = true;
                        ibmsimulations.CreateActivityThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create Activity Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Blog Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateBlog = true;
                        ibmsimulations.CreateBlogThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create Blog Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Bookmark Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateBookmark = true;
                        ibmsimulations.CreateBookmarkThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create Bookmark Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Community Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateCommunity = true;
                        ibmsimulations.CreateCommunityThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create Community Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create File Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateFile = true;
                        ibmsimulations.CreateFileThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create File Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Wiki Threshold").Count() > 0)
                    {
                        ibmsimulations.CreateWiki = true;
                        ibmsimulations.CreateWikiThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Create Wiki Threshold").Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Search Profile Threshold").Count() > 0)
                    {
                        ibmsimulations.SearchProfile = true;
                        ibmsimulations.SearchProfileThreshold = Convert.ToInt32(result.SimulationTests.FirstOrDefault(x => x.Name == "Search Profile Threshold").Value);
                    }
                }
                else
                {
                    //Default threshold values
                    ibmsimulations.CreateActivityThreshold = 10000;
                    ibmsimulations.CreateBlogThreshold = 10000;
                    ibmsimulations.CreateBookmarkThreshold = 10000;
                    ibmsimulations.CreateCommunityThreshold = 10000;
                    ibmsimulations.CreateFileThreshold = 10000;
                    ibmsimulations.CreateWikiThreshold = 10000;
                    ibmsimulations.SearchProfileThreshold = 10000;
                }
                ibmsimulations.CommunityUUID = result.ConnectionsCommunityUuid;
                ibmsimulations.TestUrl = result.ConnectionsTestUrl;
                Response = Common.CreateResponse(ibmsimulations);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting simulation tests has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("{device_id}/get_tests")]
        public APIResponse GetTests(string device_id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                var simulationSettings = new List<string> { "Mail Flow", "Create Folder", "Create Site", "OneDrive Upload", "OneDrive Download",  };
                TestsModel tests = new TestsModel();
                tests.Id = device_id;
                var result = serversRepository.Collection.AsQueryable().Where(x => x.Id == device_id).FirstOrDefault();
                if (result.SimulationTests != null)
                {
                    if (result.SimulationTests.Where(x => x.Name == "Mail Flow").Count() > 0)
                    {
                        tests.MailFlow = true;
                        var tempval = result.SimulationTests.FirstOrDefault(x => x.Name == "Mail Flow");
                        tests.MailFlowThreshold = Convert.ToInt32(tempval.Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Folder").Count() > 0)
                    {
                        tests.CreateFolder = true;
                        var tempval = result.SimulationTests.FirstOrDefault(x => x.Name == "Create Folder");
                        tests.CreateFolderThreshold = Convert.ToInt32(tempval.Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Site").Count() > 0)
                    {
                        tests.CreateSite = true;
                        var tempval = result.SimulationTests.FirstOrDefault(x => x.Name == "Create Site");
                        tests.CreateSiteThreshold = Convert.ToInt32(tempval.Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "OneDrive Upload").Count() > 0)
                    {
                        tests.OneDriveUpload = true;
                        var tempval = result.SimulationTests.FirstOrDefault(x => x.Name == "OneDrive Upload");
                        tests.OneDriveUploadThreshold = Convert.ToInt32(tempval.Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "OneDrive Download").Count() > 0)
                    {
                        tests.OneDriveDownload = true;
                        var tempval = result.SimulationTests.FirstOrDefault(x => x.Name == "OneDrive Download");
                        tests.OneDriveDownloadThreshold = Convert.ToInt32(tempval.Value);
                    }
                    if (result.SimulationTests.Where(x => x.Name == "SMTP").Count() > 0)
                    {
                        tests.SMTP = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Auto Discovery").Count() > 0)
                    {
                        tests.AutoDiscovery = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Create Calendar").Count() > 0)
                    {
                        tests.CreateCalendar = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "IMAP").Count() > 0)
                    {
                        tests.IMAP = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "POP3").Count() > 0)
                    {
                        tests.POP3 = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "MAPI Connectivity").Count() > 0)
                    {
                        tests.MAPIConnectivity = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "Inbox").Count() > 0)
                    {
                        tests.Inbox = true;
                    }
                    if (result.SimulationTests.Where(x => x.Name == "OWA").Count() > 0)
                    {
                        tests.OWA = true;
                    }
                }
                Response = Common.CreateResponse(tests);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting tests has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_tests")]
        public APIResponse SaveTests([FromBody]TestsModel tests)
        {
            List<NameValuePair> nameValuePairs = new List<NameValuePair>();
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                if (tests.MailFlow)
                    nameValuePairs.Add(new NameValuePair { Name = "Mail Flow", Value = Convert.ToString(tests.MailFlowThreshold) });
                if (tests.CreateFolder)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Folder", Value = Convert.ToString(tests.CreateFolderThreshold) });
                if (tests.CreateSite)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Site", Value = Convert.ToString(tests.CreateSiteThreshold) });
                if (tests.OneDriveUpload)
                    nameValuePairs.Add(new NameValuePair { Name = "OneDrive Upload", Value = Convert.ToString(tests.OneDriveUploadThreshold) });
                if (tests.OneDriveDownload)
                    nameValuePairs.Add(new NameValuePair { Name = "OneDrive Download", Value = Convert.ToString(tests.OneDriveDownloadThreshold) });
                if (tests.SMTP)
                    nameValuePairs.Add(new NameValuePair { Name = "SMTP" });
                if (tests.AutoDiscovery)
                    nameValuePairs.Add(new NameValuePair { Name = "Auto Discovery" });
                if (tests.CreateCalendar)
                    nameValuePairs.Add(new NameValuePair { Name = "Create Calendar" });
                if (tests.IMAP)
                    nameValuePairs.Add(new NameValuePair { Name = "IMAP" });
                if (tests.POP3)
                    nameValuePairs.Add(new NameValuePair { Name = "POP3" });
                if (tests.MAPIConnectivity)
                    nameValuePairs.Add(new NameValuePair { Name = "MAPI Connectivity" });
                if (tests.Inbox)
                    nameValuePairs.Add(new NameValuePair { Name = "Inbox" });
                if (tests.OWA)
                    nameValuePairs.Add(new NameValuePair { Name = "OWA" });
                Server server = serversRepository.Get(tests.Id);
                var updateDefination = serversRepository.Updater.Set(p => p.SimulationTests, nameValuePairs);
                var result = serversRepository.Update(server, updateDefination);
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Tests updated successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving tests has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }
        #endregion

        #region Node Health
        /// <summary>
        /// 
        /// </summary>
        /// <author>swathi</author>
        /// <returns></returns>

        [HttpGet("get_nodes_health")]
        public APIResponse GetAllNodesHealth()
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                var result = nodesRepository.Collection.AsQueryable().Select(x => new NodesModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    HostName = x.HostName,
                    Pulse = x.Pulse,
                    IsAlive = x.IsAlive,
                    Alive = x.IsAlive ? "Yes" : "No",
                    LoadFactor = x.LoadFactor,
                    IsConfiguredPrimary = x.IsConfiguredPrimary,
                    IsPrimary = x.IsPrimary,
                    Version=x.Version,
                    NodeType=x.NodeType,
                    Location=x.Location
                    



                }).ToList().OrderBy(x => x.Name);
                var nodesData = nodesRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
               
                var locations = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();

                Response = Common.CreateResponse(new { nodesData = nodesData, result = result,locations=locations });

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting nodes health has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>swathi</author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("get_nodes_services")]
        public APIResponse GetAllNodesServices(string id)
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                Expression<Func<Nodes, bool>> expression = (p => p.Id == id);
                var result = nodesRepository.Collection.AsQueryable().Where(x => x.Id == id).FirstOrDefault();
                //  var serviceresult = nodesRepository.Find(expression).Select(x => x.ServiceStatus).FirstOrDefault();
                //   var distinctData = summaryStats.Select(x => new { DeviceId = x.DeviceID, DeviceName = x.DeviceName }).Distinct().OrderBy(x => x.DeviceName).ToList();
                List<ServiceStatusModel> service = new List<ServiceStatusModel>();
                List<NodesServices> nodesServicesStatus = new List<NodesServices>();
                var serviceresult = result.ServiceStatus;
                NodesServices nodesservices = new NodesServices();
                if (serviceresult != null)
                {

                    



                    var dominoService = serviceresult.FirstOrDefault(x => x.Name == "VSService_Domino");
                    nodesservices.VSServicDomino = dominoService.State;
                    var coreService = serviceresult.FirstOrDefault(x => x.Name == "VSService_Core");
                    nodesservices.VSServiceCore = coreService.State;

                    var vsAlert = serviceresult.FirstOrDefault(x => x.Name == "VSService_Alerting");
                    nodesservices.VSServiceAlerting = vsAlert.State;

                    var clusterHealth = serviceresult.FirstOrDefault(x => x.Name == "VSService_Cluster Health");
                    nodesservices.VSServiceCluster = clusterHealth.State;

                    var dailyService = serviceresult.FirstOrDefault(x => x.Name == "VSService_Daily Service");
                    nodesservices.VSService_Daily = dailyService.State;

                    var masterService = serviceresult.FirstOrDefault(x => x.Name == "VSService_Master Service");
                    nodesservices.VSService_Master = masterService.State;

                    var dbHealth = serviceresult.FirstOrDefault(x => x.Name == "VSService_DB Health");
                    nodesservices.VSService_DB = dbHealth.State;

                    var exJournal = serviceresult.FirstOrDefault(x => x.Name == "VSService_EX Journal");
                    nodesservices.VSService_EX = exJournal.State;

                    var console = serviceresult.FirstOrDefault(x => x.Name == "VSService_Console Commands");
                    nodesservices.VSService_Console = console.State;

                    var microsoft = serviceresult.FirstOrDefault(x => x.Name == "VSService_Microsoft");
                    nodesservices.VSService_Microsoft = microsoft.State;

                    var core64Bit = serviceresult.FirstOrDefault(x => x.Name == "VSService_Core 64-bit");
                    nodesservices.VSService_Core64 = core64Bit.State;

                    //if()
                    
                }
                else
                {
                    string[] notAlwaysRunning = new string[] {
                "Daily Service",
                "DB Health"
            };

                    string[] OnlyPrimary = new String[] {
                "DB Health",
                "Alerting"
            };

                    if (OnlyPrimary.Contains(nodesservices.VSServiceAlerting) && result.IsPrimary == false)
                    {
                        nodesservices.VSServiceAlerting = "N/A";
                    }
                    if (OnlyPrimary.Contains(nodesservices.VSService_DB) && result.IsPrimary == false)
                    {
                        nodesservices.VSService_DB = "N/A";
                    }
                    if (notAlwaysRunning.Contains(nodesservices.VSService_Daily) && nodesservices.VSService_Daily != "Not Found" && result.IsAlive == true)
                    {                        
                    }
                    if (result.IsAlive==false)
                    {
                        nodesservices.VSService_DB = "N/A";
                        nodesservices.VSServiceAlerting = "N/A";
                        nodesservices.VSServicDomino = "N/A";
                        nodesservices.VSServiceCore = "N/A";
                        nodesservices.VSServiceCluster = "N/A";
                        nodesservices.VSService_Daily = "N/A";
                        nodesservices.VSService_Master = "N/A";
                        nodesservices.VSService_DB = "N/A";
                        nodesservices.VSService_EX = "N/A";
                        nodesservices.VSService_Console = "N/A";
                        nodesservices.VSService_Microsoft = "N/A";
                        nodesservices.VSService_Core64 = "N/A";
                    }
                   
                                       

                }
                nodesServicesStatus.Add(nodesservices);

                Response = Common.CreateResponse(nodesServicesStatus);


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting nodes health has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>swathi</author>
        /// <param name="nodeshealth"></param>
        /// <returns></returns>
        [HttpPut("save_nodes_health")]
        public APIResponse UpdateNodesHealth([FromBody]NodesModel nodeshealth)
        {
            FilterDefinition<Nodes> filterDef;
            UpdateDefinition<Nodes> updateDef;
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                //First, update all nodes except for the currently edited one, set is_configured_primary to false if nodeshealth.IsConfiguredPrimary == true
                if (nodeshealth.IsConfiguredPrimary == true)
                {
                    filterDef = Builders<Nodes>.Filter.Where(p => p.Id != nodeshealth.Id);
                    updateDef = nodesRepository.Updater.Set(p => p.IsConfiguredPrimary, false);
                    var result1 = nodesRepository.Update(filterDef, updateDef);
                }
                //Next, update the currently edited node
                filterDef = Builders<Nodes>.Filter.Where(p => p.Id == nodeshealth.Id);
                updateDef = nodesRepository.Updater.Set(p => p.Name, nodeshealth.Name)
                    .Set(p => p.HostName, nodeshealth.HostName)
                    .Set(p => p.Pulse, nodeshealth.Pulse)
                    .Set(p => p.IsAlive, nodeshealth.IsAlive)
                    .Set(p => p.LoadFactor, nodeshealth.LoadFactor)
                    .Set(p => p.IsConfiguredPrimary, nodeshealth.IsConfiguredPrimary)
                    .Set(p => p.IsPrimary, nodeshealth.IsPrimary)
                    .Set(p => p.Version, nodeshealth.Version)
                    .Set(p => p.NodeType, nodeshealth.NodeType)
                    .Set(p => p.Location, nodeshealth.Location);
                var result = nodesRepository.Update(filterDef, updateDef);

                var result_disp = nodesRepository.Collection.AsQueryable().Select(x => new NodesModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    HostName = x.HostName,
                    Pulse = x.Pulse,
                    IsAlive = x.IsAlive,
                    Alive = x.IsAlive ? "Yes" : "No",
                    LoadFactor = x.LoadFactor,
                    IsConfiguredPrimary = x.IsConfiguredPrimary,
                    IsPrimary = x.IsPrimary,
                    Version = x.Version,
                    NodeType = x.NodeType,
                    Location = x.Location
                }).ToList().OrderBy(x => x.Name);
                Response = Common.CreateResponse(result_disp, Common.ResponseStatus.Success.ToDescription(), "Nodes health updated successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving nodes health information has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>Swathi</author>
        /// <param name="devicesettings"></param>
        /// <returns></returns>
        [HttpPut("save_nodes_servers")]
        public APIResponse UpdateNodesServers([FromBody] DeviceSettings devicesettings)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                nodesRepository = new Repository<Nodes>(ConnectionString);
                string selectedNode = Convert.ToString(devicesettings.Setting);
                var platform = nodesRepository.Collection.AsQueryable().Where(x => x.Id == selectedNode).Select(x => x.Name).FirstOrDefault();
                var devicesList = ((Newtonsoft.Json.Linq.JArray)devicesettings.Devices).ToObject<List<string>>();
                foreach (var id in devicesList)
                {
                    if (id != "")
                    {
                        FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                        var updateDefination = serversRepository.Updater.Set(p => p.AssignedNode, platform);
                        var result = serversRepository.Update(filterDefination, updateDefination);
                    }    
                }
                foreach (var id in devicesList)
                {
                    if (id != "")
                    {
                        FilterDefinition<ServerOther> filterDefination2 = Builders<ServerOther>.Filter.Where(p => p.Id == id);
                        var updateDefination2 = serverOtherRepository.Updater.Set(p => p.AssignedNode, platform);
                        var result = serverOtherRepository.Update(filterDefination2, updateDefination2);
                    }
                }
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Nodes assigned successfully");
            }


            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Node assignment has failed.\n Error Message :" + exception.Message);
            }
            return Response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>Swathi</author>
        /// <param name="Id"></param>
        [HttpDelete("delete_nodes_health/{Id}")]
        public void DeleteNodesHealth(string Id)
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                Expression<Func<Nodes, bool>> expression = (p => p.Id == Id);
                nodesRepository.Delete(expression);


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Deletion of nodes health has failed.\n Error Message :" + exception.Message);
            }
        }


        #endregion

        #region Mobile Users
        /// <summary>
        ///Get all Mobile Users
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_mobile_users")]
        public APIResponse GetMobileUsers()
        {
            try
            {
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var result = mobileDevicesRepository.All().Where(x => x.ThresholdSyncTime != null).Select(x => new MobileUserDevice
                {
                    UserName = x.UserName,
                    DeviceName = x.DeviceName,
                    DeviceId = x.DeviceID,
                    ThresholdSyncTime = x.ThresholdSyncTime,
                    Id = x.Id


                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting mobile user records has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        /// <summary>
        ///Delete Mobile User
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete_mobile_users/{Id}")]
        public APIResponse DeleteMobileUser(string Id)
        {
            try
            {
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);

                FilterDefinition<MobileDevices> filterDefination = Builders<MobileDevices>.Filter.Where(p => p.Id == Id);
                var updateDefination = mobileDevicesRepository.Updater.Set(p => p.ThresholdSyncTime, null);
                mobileDevicesRepository.Update(filterDefination, updateDefination);
                var result = GetMobileDeviceList();
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Mobile user removed from monitoring successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Removing mobile user from monitoring has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion


        #region Mobile Devices
        /// <summary>
        ///Returns all mobile devices
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all_mobile_devices")]
        public APIResponse GetALLMobileDevices()
        {
            try
            {
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var result = mobileDevicesRepository.All().Where(x => x.ThresholdSyncTime == null && (x.IsActive.HasValue && x.IsActive.Value)).Select(x => new MobileUserDevice
                {
                    UserName = x.UserName,
                    DeviceName = x.DeviceName,
                    DeviceId = x.DeviceID,
                    OperatingSystem = x.OSType,
                    Id = x.Id


                }).ToList().OrderBy(x => x.UserName);


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting mobile devices has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///Save Mobile Users
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_mobileusers")]
        public APIResponse SaveMobileUsers([FromBody]MobileUserDevice mobileUser)
        {
            try
            {
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);

                if (!string.IsNullOrEmpty(mobileUser.Id))
                {
                    FilterDefinition<MobileDevices> filterDefination = Builders<MobileDevices>.Filter.Where(p => p.Id == mobileUser.Id);
                    var updateDefination = mobileDevicesRepository.Updater.Set(p => p.ThresholdSyncTime, mobileUser.ThresholdSyncTime);
                    mobileDevicesRepository.Update(filterDefination, updateDefination);
                    // need to return two data sets on save and split them up in the ui to refresh the grids
                    var result = GetMobileDeviceList();
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Mobile users updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Saving mobile users information has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        private List<dynamic> GetMobileDeviceList()
        {
            List<dynamic> result_disp = new List<dynamic>();
            List<dynamic> result_critical = new List<dynamic>();
            List<dynamic> result_all = new List<dynamic>();
            
            try
            {
                mobiledevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var mobileUsers = mobiledevicesRepository.Collection.AsQueryable().ToList();
                foreach (var mobileUser in mobileUsers)
                {
                    if (mobileUser.ThresholdSyncTime != null)
                    {
                        result_critical.Add(new MobileUserDevice
                        {
                            Id = mobileUser.Id.ToString(),
                            UserName = mobileUser.UserName,
                            DeviceName = mobileUser.DeviceName,
                            DeviceId = mobileUser.DeviceID,
                            ThresholdSyncTime = mobileUser.ThresholdSyncTime
                        });
                    }
                    else
                    {
                        result_all.Add(new MobileUserDevice
                        {
                            Id = mobileUser.Id.ToString(),
                            UserName = mobileUser.UserName,
                            DeviceName = mobileUser.DeviceName,
                            DeviceId = mobileUser.DeviceID,
                            OperatingSystem = mobileUser.OSType
                        });
                    }
                }
                result_disp.Add(result_critical);
                result_disp.Add(result_all);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result_disp;
        }

        #endregion

        #region Issues
        /// <summary>
        ///Returns all open issues
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpGet("get_all_open_issues")]
        public APIResponse GetALLOpenIssues()
        {
            DateTime? nullDate = null;
            try
            {
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                var result = eventsdetectedRepository.All().Where(x => x.EventDismissed == null).Select(x => new AlertsModel
                {
                    DeviceType = x.DeviceType,
                    DeviceName = x.Device,
                    EventType = x.EventType,
                    Details = x.Details,
                    EventDetected = x.EventDetected != null ? x.EventDetected.Value : nullDate

                }).OrderByDescending(x => x.EventDetected).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting all open issues has failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Servers Import
        #region Domino
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="serverImport"></param>
        /// <returns></returns>
        // [FunctionAuthorize("DominoServerImport")]
        [HttpPut("load_domino_servers")]
        public APIResponse LoadDominoServers([FromBody]DominoServerImportModel serverImport)
        {

            try
            {
                Domino.NotesDbDirectory notesDBDirectory;
                Domino.NotesDatabase notesDatabase;
                Domino.NotesView notesView;
                Domino.NotesDocument notesDocument;
                Domino.NotesName notesName;
                Domino.NotesItem notesItem;
                Domino.NotesItem item2;
                byte[] password;
                string dominoPassword; //should be string
                string encryptedPassword = string.Empty;
                string errorMessage = string.Empty;

                //Save the prumary server value in name_value collection
                List<NameValue> nameValueList = new List<NameValue>();
                bool isNamevalueSaved = Common.SaveNameValue(new NameValue { Name = "Primary Server", Value = serverImport.DominoServer });
                VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();

                locationRepository = new Repository<Location>(ConnectionString);
                var locationList = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();


                if (locationList.Count > 0)
                {
                    try
                    {
                        var passwordName = Common.GetNameValue("Password");

                        if (passwordName != null)
                            encryptedPassword = passwordName.Value;
                        if (!string.IsNullOrEmpty(encryptedPassword))
                        {
                            var passwordArray = encryptedPassword.Split(',');
                            password = new byte[passwordArray.Length];
                            for (int i = 0; i < passwordArray.Length; i++)
                            {
                                password[i] = Byte.Parse(passwordArray[i]);
                            }
                        }
                        else
                        {
                            errorMessage = "Notes password may not be empty. Please update the password under Stored Passwords & Options\\IBM Domino Settings.";
                            throw new Exception(errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "The following error has occurred: " + ex.Message;
                        password = null;
                        throw new Exception(errorMessage);
                    }
                    try
                    {
                        if (password != null)
                        {
                            dominoPassword = mySecrets.Decrypt(password);
                        }
                        else
                        {
                            dominoPassword = null;
                        }
                    }
                    catch (Exception ex)
                    {
                        errorMessage = "The following error has occurred: " + ex.Message;
                        dominoPassword = string.Empty;
                        throw new Exception(errorMessage);
                    }

                    if (!string.IsNullOrEmpty(dominoPassword))
                    {
                        try
                        {
                            Domino.NotesSession NotesSessionObject = new Domino.NotesSession();
                            NotesSessionObject.Initialize(dominoPassword);
                            notesDBDirectory = NotesSessionObject.GetDbDirectory(serverImport.DominoServer); ;
                            notesDatabase = notesDBDirectory.OpenDatabase("names.nsf");
                            notesView = notesDatabase.GetView("($Servers)");
                            notesDocument = notesView.GetFirstDocument();
                            List<ServersModel> serverList = new List<ServersModel>();
                            serversRepository = new Repository<Server>(ConnectionString);
                            var existingServerList = serversRepository.Collection.AsQueryable().Select(x => new ServersModel
                            {
                                DeviceName = x.DeviceName,
                                DeviceType = x.DeviceType,
                                Description = x.Description
                                // DeviceId = x.Id
                            }).ToList();

                            while (notesDocument != null)
                            {
                                ServersModel server = new ServersModel();
                                notesItem = notesDocument.GetFirstItem("ServerName");
                                notesName = NotesSessionObject.CreateName(notesItem.Text);
                                server.DeviceName = notesName.Abbreviated;

                                var matchedServers = existingServerList.Where(x => x.DeviceName == server.DeviceName).Count();
                                if (matchedServers == 0)
                                {
                                    notesItem = notesDocument.GetFirstItem("SMTPFullHostDomain");

                                    if (notesItem == null || notesItem.Text == null || notesItem.Text == "")
                                    {
                                        item2 = notesDocument.GetFirstItem("NetAddresses");

                                        if (item2 == null || item2.Text == null || item2.Text == "")
                                        {
                                            server.IpAddress = "dummyaddress.yourdomain.com";
                                        }
                                        else
                                        { server.IpAddress = item2.Text; }
                                    }
                                    else
                                    { server.IpAddress = notesItem.Text; }
                                    serverList.Add(server);
                                }
                                notesDocument = notesView.GetNextDocument(notesDocument);
                            }
                            //serverList.Add(new ServersModel {DeviceName="Server1",IpAddress="19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server2", IpAddress = "19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server3", IpAddress = "19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server4", IpAddress = "19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server5", IpAddress = "19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server6", IpAddress = "19.10.1.125" });
                            //serverList.Add(new ServersModel { DeviceName = "Server7", IpAddress = "19.10.1.125" });
                            if (serverList.Count > 0)
                            {

                                Response = Common.CreateResponse(new { locationList = locationList, serverList = serverList });
                            }
                            else
                            {
                                errorMessage = "There are no new servers in the address book that have not already been imported into VitalSigns.";
                                throw new Exception(errorMessage);
                            }
                        }
                        catch (Exception ex)
                        {
                            errorMessage = "The following error has occurred: " + ex.Message;
                            throw new Exception(errorMessage);

                        }
                    }
                }
                //5/13/2014 NS added for VSPLUS-183
                else
                {
                    errorMessage = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the Configurator -> Application Settings -> Locations menu option.";
                    throw new Exception(errorMessage);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }
        //  [FunctionAuthorize("DominoServerImport")]
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_domino_import")]
        public APIResponse GetDominoImportData()
        {
            DominoServerImportModel model = new DominoServerImportModel();
            if (Common.GetNameValue("Primary Server") != null)
            {
                model.DominoServer = Common.GetNameValue("Primary Server").Value;

            }
            model.Location = null;

            deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
            model.DeviceAttributes = deviceAttributesRepository.All().Where(x => (x.DeviceType == "Domino") && (x.Category == "Scan Settings" || x.Category == "Mail Settings")).Select(x => new DeviceAttributesModel
            {
                Id = x.Id,
                AttributeName = x.AttributeName,
                DefaultValue = x.DefaultValue,
                DeviceType = x.DeviceType,
                FieldName = x.FieldName,
                Category = x.Category,
                DataType = x.DataType,
                Type = x.Type,
                Unitofmeasurement = x.Unitofmeasurement,
                IsSelected = false
            }).OrderBy(x => x.AttributeName).ToList();


            model.CpuThreshold = 90;
            model.MemoryThreshold = 90;
            dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
            model.ServerTasks = dominoservertasksRepository.All().Select(x => new DominoServerTasksModel
            {
                Id = x.Id,
                IsSelected = false,
                TaskName = x.TaskName,
                IsLoad = false,
                IsRestartASAP = false,
                IsResartLater = false,
                IsDisallow = false
            }).OrderBy(x => x.TaskName).ToList();


            foreach (var attri in model.DeviceAttributes)
            {
                if (attri.DataType == "bool" && (attri.DefaultValue == "false" || attri.DefaultValue == "0"))
                {
                    attri.DefaultboolValues = false;
                }
                else
                {
                    attri.DefaultboolValues = true;
                }
            }
            return Common.CreateResponse(model);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="serverImport"></param>
        /// <returns></returns>
        [HttpPut("save_domino_servers")]
        public APIResponse SaveDominoServers([FromBody]DominoServerImportModel serverImport)
        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);

                foreach (var serverModel in serverImport.Servers)
                {
                    if (serverModel.IsSelected)
                    {
                        Server server = new Server();
                        server.Id = ObjectId.GenerateNewId().ToString();
                        server.DeviceName = serverModel.DeviceName;
                        server.DeviceType = "Domino";
                        server.LocationId = serverImport.Location;
                        server.IsEnabled = true;
                        server.MemoryThreshold = serverImport.MemoryThreshold != null ? Math.Round(Convert.ToDouble(serverImport.MemoryThreshold)/100, 1) : 0.9;
                        server.CpuThreshold = serverImport.CpuThreshold != null ? Math.Round(Convert.ToDouble(serverImport.CpuThreshold) / 100, 1) : 0.9;
                        List<DominoServerTask> ServerTasks = new List<DominoServerTask>();
                        foreach (var serverTask in serverImport.ServerTasks)
                        {
                            if (serverTask.IsSelected ?? false)
                            {
                                DominoServerTask dominoServerTask = new DominoServerTask();
                                dominoServerTask.Id = ObjectId.GenerateNewId().ToString();
                                dominoServerTask.TaskId = serverTask.TaskId;
                                dominoServerTask.TaskName = serverTask.TaskName;
                                dominoServerTask.SendLoadCmd = false;
                                dominoServerTask.Monitored = false;
                                dominoServerTask.SendRestartCmd = false;
                                dominoServerTask.SendRestartCmdOffhours = false;
                                dominoServerTask.SendExitCmd = false;
                                if (server.ServerTasks != null)
                                    ServerTasks = server.ServerTasks;
                                ServerTasks.Add(dominoServerTask);
                            }
                        }
                        server.ServerTasks = ServerTasks;
                        serversRepository.Insert(server);

                        Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                        var updateBuilder = Builders<BsonDocument>.Update;
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(server.Id));
                        UpdateDefinition<BsonDocument> updateDefinition = updateBuilder.Set("is_enabled", true);
                        foreach (var attribute in serverImport.DeviceAttributes)
                        {
                            if (!string.IsNullOrEmpty(attribute.FieldName))
                            {
                                string field = attribute.FieldName;
                                string value = attribute.DefaultValue;
                                string datatype = attribute.DataType;
                                if (datatype == "int")
                                {
                                    int outputvalue = Convert.ToInt32(value);
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, outputvalue) : updateDefinition.Set(field, outputvalue);
                                }
                                if (datatype == "double")
                                {
                                    double outputvalue = Convert.ToDouble(value);
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, outputvalue) : updateDefinition.Set(field, outputvalue);
                                }
                                if (datatype == "bool")
                                {
                                    bool booloutput;
                                    if (value == "0")
                                    {
                                        booloutput = false;
                                    }
                                    else
                                    {
                                        booloutput = true;
                                    }
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, booloutput) : updateDefinition.Set(field, booloutput);
                                }
                                if (datatype == "string")
                                {
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, value) : updateDefinition.Set(field, value);
                                }
                            }
                        }

                        repository.Collection.UpdateMany(filter, updateDefinition);
                    }
                }
                //2/24/2017 NS added for VSPLUS-3506
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse("Success", Common.ResponseStatus.Success.ToDescription(), "Servers imported successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server import has failed. \n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Microsoft Servers
        [HttpGet("get_microsoft_import")]
        public APIResponse GetMicrosoftImportData( string device_type="")
        {
           MicrosoftServerImportModel model = new MicrosoftServerImportModel();

            deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
            model.DeviceAttributes = deviceAttributesRepository.All().Where(x => (x.DeviceType == "device_type") && (x.Category == "Scan Settings" || x.Category == "Mail Settings")).Select(x => new DeviceAttributesModel
            {
                Id = x.Id,
                AttributeName = x.AttributeName,
                DefaultValue = x.DefaultValue,
                DeviceType = x.DeviceType,
                FieldName = x.FieldName,
                Category = x.Category,
                DataType = x.DataType,
                Type = x.Type,
                Unitofmeasurement = x.Unitofmeasurement,
                IsSelected = false
            }).OrderBy(x => x.AttributeName).ToList();
            model.CpuThreshold = 90;
            model.MemoryThreshold = 90;
            model.ReplyQueueThreshold = 2;
            model.CopyQueueThreshold = 2;
            foreach (var attri in model.DeviceAttributes)
            {
                if (attri.DataType == "bool" && (attri.DefaultValue == "false" || attri.DefaultValue == "0"))
                {
                    attri.DefaultboolValues = false;
                }
                else
                {
                    attri.DefaultboolValues = true;
                }
            }
            return Common.CreateResponse(model);
        }

        [HttpPut("save_microsoft_servers")]
        public APIResponse SaveMicrosoftServers([FromBody]MicrosoftServerImportModel serverImport,string device_type= "")
        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                foreach (var serverModel in serverImport.Servers)
                {
                    if (serverModel.IsSelected)
                    {
                        Server server = new Server();
                        server.Id = ObjectId.GenerateNewId().ToString();
                        server.DeviceName = serverModel.DeviceName;
                        server.DeviceType = device_type;
                        server.LocationId = serverImport.Location;
                        server.IsEnabled = true;
                        server.IPAddress = serverImport.Protocol+ serverModel.IpAddress;
                        if (server.DeviceType == Enums.ServerType.Exchange.ToDescription().ToString())
                        { server.AuthenticationType = serverImport.AuthenticationType; }
                        if (server.DeviceType != Enums.ServerType.DatabaseAvailabilityGroup.ToDescription().ToString())
                        {
                            server.MemoryThreshold = serverImport.MemoryThreshold != null ? Math.Round(Convert.ToDouble(serverImport.MemoryThreshold) / 100, 1) : 0.9;
                            server.CpuThreshold = serverImport.CpuThreshold != null ? Math.Round(Convert.ToDouble(serverImport.CpuThreshold) / 100, 1) : 0.9;
                        }
                        if (server.DeviceType == Enums.ServerType.DatabaseAvailabilityGroup.ToDescription().ToString())
                        {
                            server.ReplyQueueThreshold = serverImport.ReplyQueueThreshold;
                            server.CopyQueueThreshold = serverImport.CopyQueueThreshold;
                            server.PrimaryServerId = serverImport.PrimaryServer;
                            server.BackupServerId = serverImport.BackupServer;
                        }
                        serversRepository.Insert(server);
                        Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                        var updateBuilder = Builders<BsonDocument>.Update;
                        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(server.Id));
                        UpdateDefinition<BsonDocument> updateDefinition = updateBuilder.Set("is_enabled", true);
                        foreach (var attribute in serverImport.DeviceAttributes.Where(x => new string[] { "Scan Settings" }.Contains(x.Category)))
                        {
                            if (!string.IsNullOrEmpty(attribute.FieldName))
                            {
                                string field = attribute.FieldName;
                                string value = attribute.DefaultValue;
                                string datatype = attribute.DataType;
                                if (datatype == "int")
                                {
                                    int outputvalue = Convert.ToInt32(value);
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, outputvalue) : updateDefinition.Set(field, outputvalue);
                                }
                                if (datatype == "double")
                                {
                                    double outputvalue = Convert.ToDouble(value);
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, outputvalue) : updateDefinition.Set(field, outputvalue);
                                }
                                if (datatype == "bool")
                                {
                                    bool booloutput;
                                    if (value == "0")
                                    {
                                        booloutput = false;
                                    }
                                    else
                                    {
                                        booloutput = true;
                                    }
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, booloutput) : updateDefinition.Set(field, booloutput);
                                }
                                if (datatype == "string")
                                {
                                    updateDefinition = updateDefinition == null ? updateBuilder.Set(field, value) : updateDefinition.Set(field, value);
                                }
                            }
                        }

                        repository.Collection.UpdateMany(filter, updateDefinition);
                    }
                }
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse("Success", Common.ResponseStatus.Success.ToDescription(), "Servers imported successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server import has failed. \n Error Message :" + exception.Message);
            }
            return Response;
        }


        [HttpPut("load_microsoft_servers")]
        public APIResponse LoadMicrosoftServers([FromBody]MicrosoftServerImportModel serverImport)
        {
            //define ps object
            System.Management.Automation.PowerShell ps = null;
            try
            {
                //decrept the credentilas
                //Server server = new Server();
                MicrosoftServerImportModel returnobject = new MicrosoftServerImportModel();
                serverRepository = new Repository<Server>(ConnectionString);
                FilterDefinition<Server> filterDefServer = serverRepository.Filter.Eq(x => x.DeviceType, serverImport.DeviceType);
                List<Server> listOfServers = serverRepository.Find(filterDefServer).ToList();
                var devicename = listOfServers.Select(x => x.DeviceName).ToList();
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                FilterDefinition<Credentials> filterDefCredentials = credentialsRepository.Filter.Eq(x => x.Id, serverImport.CredentialId);
                List<Credentials> listOfCredentials = credentialsRepository.Find(filterDefCredentials).ToList();
                Credentials creds = listOfCredentials.First();
                VSFramework.TripleDES tripleDes = new VSFramework.TripleDES();
                string cmd = "";
                if (serverImport.DeviceType == Enums.ServerType.Exchange.ToDescription().ToString())
                {
                    ps = MicrosoftConnections.ConnectToExchange(serverImport.IpAddress, creds.UserId, tripleDes.Decrypt(creds.Password), serverImport.IpAddress, serverImport.AuthenticationType);
                    cmd = "Get-ExchangeServer | select Name, Fqdn | sort name";
                }
                else if (serverImport.DeviceType == Enums.ServerType.DatabaseAvailabilityGroup.ToDescription().ToString())
                {

                    ps = MicrosoftConnections.ConnectToExchange(serverImport.IpAddress, creds.UserId, tripleDes.Decrypt(creds.Password), serverImport.IpAddress, serverImport.AuthenticationType);
                    //cmd = "Get-ExchangeServer | select Name, Fqdn | sort name";
                    cmd = "Get-DatabaseAvailabilityGroup | Select @{ Name = 'Name'; Expression ={$_.Name} },@{ Name = 'Fqdn'; Expression ={$_.WitnessServer} } | Sort Name";

                }
                else
                {
                    throw new Exception("Device Type is not supported");
                }

                //depending on server type call appropriate function

                //refre back to vs web 365(make logic here)
                
                ps.AddScript(cmd);
                var results = ps.Invoke();
                //loop through and add it to new array {temserverlist array}
                returnobject.Servers = new List<ServersModel>();

                foreach (System.Management.Automation.PSObject psobject in results)
                {
                    try
                    {
                        string name = psobject.Properties["Name"].Value.ToString();
                        string Fqdn = psobject.Properties["Fqdn"].Value.ToString();
                        var exchnageservers = new ServersModel();
                        exchnageservers.DeviceName = name;
                        exchnageservers.IpAddress = Fqdn;
                        if (devicename.Contains(name))
                            continue;
                        returnobject.Servers.Add(exchnageservers);
                       
                    }

                    catch (Exception ex)
                    {
                        throw ex;

                    }

                }
                locationRepository = new Repository<Location>(ConnectionString);
                returnobject.LocationList = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
                if (serverImport.DeviceType == Enums.ServerType.DatabaseAvailabilityGroup.ToDescription())
                {
                    serversRepository = new Repository<Server>(ConnectionString);
                    returnobject.Exchangelist = serversRepository.Find(x => x.DeviceType == Enums.ServerType.Exchange.ToDescription()).ToList()
                        .Select(x => new ComboBoxListItem {Value = x.Id, DisplayText = x.DeviceName }).ToList();
                }

                returnobject.DeviceAttributes = GetDeviceAttributeList(serverImport.DeviceType);
                Response = Common.CreateResponse(returnobject);

                return Response;
            }
           
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server import has failed. \n Error Message :" + exception.Message);
            }
            finally
            {

            }
            return Response;
            
        }

        //var TempServerList = new List<ServersModel>()
        //{
        //    new ServersModel(){ DeviceName = "text1", IpAddress ="dummy@dummy.com" },
        //    new ServersModel(){ DeviceName = "text2", IpAddress ="dummy@dummy.com" },
        //       new ServersModel(){ DeviceName = "text3", IpAddress ="dummy@dummy.com" },
        //          new ServersModel(){ DeviceName = "text4", IpAddress ="dummy@dummy.com" },
        //             new ServersModel(){ DeviceName = "text5", IpAddress ="dummy@dummy.com" }


        //};
       
        #endregion

        #region WebSphere
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="cellInfo"></param>
        /// <returns></returns>

        [HttpPut("get_websohere_nodes")]
        public APIResponse LoadWebSphereNodes([FromBody]CellInfo cellInfo)
        {

            try
            {
                if (logUtils == null) logUtils = new LogUtilities.LogUtils();
                BusinessHours bh = new BusinessHours();
                byte[] password;
                string decryptedPassword = string.Empty;
                string errorMessage = string.Empty;
                serversRepository = new Repository<Server>(ConnectionString);
                //Get user name and password from credentials

                try
                {
                    credentialsRepository = new Repository<Credentials>(ConnectionString);

                    var credential = credentialsRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == cellInfo.CredentialsId);
                    if (credential != null)
                    {
                        if (!string.IsNullOrEmpty(credential.Password))
                        {
                            var passwordArray = credential.Password.Split(',');
                            password = new byte[passwordArray.Length];
                            for (int i = 0; i < passwordArray.Length; i++)
                            {
                                password[i] = Byte.Parse(passwordArray[i]);
                            }
                            VSFramework.TripleDES mySecrets = new VSFramework.TripleDES();
                            cellInfo.Password = mySecrets.Decrypt(password);
                            cellInfo.UserName = credential.UserId;


                            VitalSignsWebSphereDLL.VitalSignsWebSphereDLL dll = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL();
                            VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties cellprop = new VitalSignsWebSphereDLL.VitalSignsWebSphereDLL.CellProperties();
                            cellprop.HostName = cellInfo.HostName;
                            cellprop.Port = Convert.ToInt32(cellInfo.PortNo == null ? 0 : cellInfo.PortNo);
                            cellprop.ConnectionType = cellInfo.ConnectionType;
                            cellprop.UserName = cellInfo.UserName;
                            cellprop.Password = cellInfo.Password;
                            cellprop.Realm = cellInfo.Realm;

                            nameValueRepository = new Repository<NameValue>(ConnectionString);
                            var filterdef = Builders<NameValue>.Filter.Where(p => p.Name == "WebSphereAppClientPath");
                            var apppath = nameValueRepository.Find(filterdef).Select(x => x.Value).FirstOrDefault();
                            var AppClientPath = "";
                            if (apppath != null)
                            {
                                AppClientPath = apppath.ToString();
                            }
                            filterdef = Builders<NameValue>.Filter.Where(p => p.Name == "InstallLocation");
                            var servicepath = nameValueRepository.Find(filterdef).Select(x => x.Value).FirstOrDefault();
                            var ServicePath = "";
                            if (servicepath != null)
                            {
                                ServicePath = servicepath.ToString();
                            }
                            LogUtilities.LogUtils.WriteDeviceHistoryEntry("All", "API", DateTime.Now.ToString() + " Before WAS DLL call");
                            var cells = dll.getServerList(cellprop, AppClientPath, ServicePath);
                            LogUtilities.LogUtils.WriteDeviceHistoryEntry("All", "API", DateTime.Now.ToString() + " After WAS DLL call");
                            System.Threading.Thread.Sleep(5000);



                            //var cells = getServerList(cellInfo);
                            foreach (var cell in cells.Cell)
                            {
                                List<WebSphereNode> nodes = new List<WebSphereNode>();

                                foreach (var cellNode in cell.Nodes.Node)
                                {
                                    WebSphereNode node = new WebSphereNode();
                                    node.NodeId = ObjectId.GenerateNewId().ToString();
                                    node.NodeName = cellNode.Name;
                                    node.HostName = cellNode.HostName;
                                    node.WebSphereServers = new List<WebSphereServer>();
                                    foreach (var nodeServer in cellNode.Servers.Server)
                                    {
                                        WebSphereServer webSphereServer = new WebSphereServer();
                                        webSphereServer.ServerId = ObjectId.GenerateNewId().ToString();
                                        webSphereServer.ServerName = nodeServer;
                                        node.WebSphereServers.Add(webSphereServer);

                                    }
                                    nodes.Add(node);
                                }
                                FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.DeviceName == cellInfo.Name);
                                var updateDefination = serversRepository.Updater.Set(p => p.CellName, cell.Name).Set(p => p.Nodes, nodes);
                                var result = serversRepository.Update(filterDefination, updateDefination);
                                Response = GetWebSphereImportData();
                            }
                        }
                        else
                        {
                            errorMessage = "Credentials may not be empty. Please enter credentials under Application Settings\\Credentials.";
                            throw new Exception(errorMessage);
                        }

                    }

                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, "Error", "Getting WebSphere nodes has failed.\n Error Message :" + exception.Message);
                }

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Getting WebSphere nodes has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="cellProperties"></param>
        /// <returns></returns>
        public Cells getServerList(CellInfo cellProperties)
        {

            try
            {

                VSFramework.RegistryHandler registry = new VSFramework.RegistryHandler();
                string AppClientPath = "";
                string ServicePath = "";

                try
                {
                    AppClientPath = registry.ReadFromRegistry("WebSphereAppClientPath").ToString();
                }
                catch
                {
                    AppClientPath = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
                }

                if (!AppClientPath.EndsWith("\\"))
                    AppClientPath += "\\";

                //WS switched to use registry value in case of different paths on HA installs
                try
                {
                    ServicePath = registry.ReadFromVitalSignsComputerRegistry("InstallPath").ToString();
                }
                catch (Exception ex)
                {
                    //ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
                }

                if (ServicePath == "")
                {
                    try
                    {
                        ServicePath = registry.ReadFromRegistry("InstallLocation").ToString();
                    }
                    catch
                    {
                        //ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";
                    }
                }

                if (ServicePath == "")
                    ServicePath = "C:\\Program Files (x86)\\VitalSignsPlus\\";

                if (!ServicePath.EndsWith("\\"))
                    ServicePath += "\\";


                ExecuteGetServerListCmd(cellProperties, AppClientPath, ServicePath);

                string filePath = ServicePath + "VitalSigns\\xml\\AppServerList.xml";

                Cells cells = (Cells)DecodeXMLFromPath(filePath, typeof(Cells));



                return cells;
            }

            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="cellProperties"></param>
        /// <param name="AppClientFolder"></param>
        /// <param name="ServicePath"></param>
        private void ExecuteGetServerListCmd(CellInfo cellProperties, string AppClientFolder, string ServicePath)
        {

            //string AppClientFolder = "C:\\Program Files (x86)\\IBM\\WebSphere\\AppClient\\";
            string pathToBatch = "GET_SERVER_LIST.bat";

            string arguments = "";
            arguments += " \"" + cellProperties.HostName + "\"";
            arguments += " \"" + cellProperties.PortNo + "\"";
            arguments += " \"" + cellProperties.ConnectionType + "\"";
            arguments += " \"" + cellProperties.UserName + "\"";
            arguments += " \"" + cellProperties.Password + "\"";
            arguments += " \"" + cellProperties.Realm + "\"";
            arguments += " \"" + AppClientFolder + "\"";

            ExecuteCommand(pathToBatch + "" + arguments, AppClientFolder, ServicePath, 60);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="cmd"></param>
        /// <param name="AppClientFolder"></param>
        /// <param name="ServicePath"></param>
        /// <param name="timeoutSec"></param>
        private void ExecuteCommand(string cmd, string AppClientFolder, string ServicePath, int timeoutSec = 60)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
            startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
            startInfo.FileName = "cmd.exe";
            startInfo.Arguments = "/C " + cmd;
            //throw new Exception(cmd);
            startInfo.WorkingDirectory = ServicePath;
            // *** Redirect the output ***
            startInfo.RedirectStandardError = true;
            startInfo.RedirectStandardOutput = true;
            startInfo.UseShellExecute = false;
            startInfo.CreateNoWindow = true;
            //startInfo.UserName = "wasadmin";
            //System.Security.SecureString str = new System.Security.SecureString();
            //"W@sadm1n".ToCharArray().ToList().ForEach(str.AppendChar);
            //startInfo.Password = str;
            process.StartInfo = startInfo;
            process.Start();


            if (!process.WaitForExit(300 * 1000))
                throw new Exception("Process did not complete in the specified time");

            //for debugging
            string s = process.StandardOutput.ReadToEnd();
            string p = process.StandardError.ReadToEnd();
            //throw new Exception(s + "...." + p);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="pathToXML"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private object DecodeXMLFromPath(string pathToXML, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);

            XmlNodeReader reader = new XmlNodeReader(doc);

            object obj = serializer.Deserialize(reader);

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpGet("get_websohere_import")]
        public APIResponse GetWebSphereImportData()
        {
            try
            {
                LogUtilities.LogUtils.WriteDeviceHistoryEntry("All", "API", DateTime.Now.ToString() + " In get_websohere_import");
                WebShpereServerImport model = new WebShpereServerImport();
                var cellsData = new List<CellInfo>();
                serversRepository = new Repository<Server>(ConnectionString);
                var servers = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType == Enums.ServerType.WebSphereCell.ToDescription()).ToList();
                var nodesData = new List<NodeInfo>();
                foreach (var server in servers)
                {
                    CellInfo cell = new CellInfo();
                    cell.DeviceId = server.Id;
                    cell.CellId = server.Id;
                    cell.CellName = server.CellName;
                    cell.Name = server.DeviceName;
                    cell.HostName = server.CellHostName;
                    cell.PortNo = server.PortNumber;
                    cell.ConnectionType = server.ConnectionType;
                    cell.GlobalSecurity = server.GlobalSecurity;
                    cell.CredentialsId = server.CredentialsId;
                    cell.Realm = server.Realm;
                    if (server.Nodes != null)
                    {
                        cell.NodesData = new List<NodeInfo>();
                        foreach (var webSphereNode in server.Nodes)
                        {
                            foreach (var webSphereServer in webSphereNode.WebSphereServers)
                            {
                                if (serversRepository.Collection.AsQueryable().Where(x => x.Id == webSphereServer.ServerId).Count() == 0)
                                {
                                    NodeInfo node = new NodeInfo();
                                    node.NodeId = webSphereNode.NodeId;
                                    node.NodeName = webSphereNode.NodeName;
                                    node.ServerId = webSphereServer.ServerId;
                                    node.ServerName = webSphereServer.ServerName + " [" + cell.Name + "~" + node.NodeName + "]";
                                    node.HostName = webSphereNode.HostName;
                                    node.CellId = cell.CellId;
                                    node.CellName = cell.Name;
                                    nodesData.Add(node);
                                }
                            }
                        }
                    }
                    cellsData.Add(cell);
                }


                deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
                model.DeviceAttributes = deviceAttributesRepository.All().Where(x => (x.DeviceType == Enums.ServerType.WebSphere.ToDescription())).Select(x => new DeviceAttributesModel
                {
                    Id = x.Id,
                    AttributeName = x.AttributeName,
                    DefaultValue = x.DefaultValue,
                    DeviceType = x.DeviceType,
                    FieldName = x.FieldName,
                    Category = x.Category,
                    DataType = x.DataType,
                    Type = x.Type,
                    Unitofmeasurement = x.Unitofmeasurement,
                    IsSelected = false
                }).OrderBy(x => x.AttributeName).ToList();

                credentialsRepository = new Repository<Credentials>(ConnectionString);

                var credentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
                foreach (var item in cellsData)
                {
                    var credential = credentialsData.FirstOrDefault(x => x.Value == item.CredentialsId);
                    if (credential != null)
                        item.CredentialsName = credential.DisplayText;
                }
                model.SelectedServers = new List<NodeInfo>();
                Response = Common.CreateResponse(new { websphereData = model, cellData = cellsData, credentialsData = credentialsData, nodeData = nodesData });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="cellInfo"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("save_websphere_cell")]
        public APIResponse SaveWebsphereCellInfo([FromBody]CellInfo cellInfo, string id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                if (string.IsNullOrEmpty(cellInfo.CellId))
                {
                    Server server = new Server();
                    server.Id = ObjectId.GenerateNewId().ToString();
                    server.CellId = ObjectId.GenerateNewId().ToString();
                    server.CellName = cellInfo.CellName;
                    server.DeviceName = cellInfo.Name;
                    server.CellHostName = cellInfo.HostName;
                    server.ConnectionType = cellInfo.ConnectionType;
                    server.PortNumber = cellInfo.PortNo;
                    server.GlobalSecurity = cellInfo.GlobalSecurity;
                    server.CredentialsId = cellInfo.CredentialsId;
                    server.Realm = cellInfo.Realm;
                    server.SametimeId = id;

                    server.DeviceType = Enums.ServerType.WebSphereCell.ToDescription();
                    var serverId = serversRepository.Insert(server);
                    Response = Common.CreateResponse(serverId, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell inserted successfully");
                }
                else
                {

                    FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == cellInfo.CellId);
                    var updateDefination = serversRepository.Updater.Set(p => p.CellId, cellInfo.CellId)
                                                             .Set(p => p.CellName, cellInfo.CellName)
                                                             .Set(p => p.DeviceName, cellInfo.Name)
                                                             .Set(p => p.CellHostName, cellInfo.HostName)
                                                             .Set(p => p.ConnectionType, cellInfo.ConnectionType)
                                                             .Set(p => p.PortNumber, cellInfo.PortNo)
                                                             .Set(p => p.GlobalSecurity, cellInfo.GlobalSecurity)
                                                             .Set(p => p.CredentialsId, cellInfo.CredentialsId)
                                                             .Set(p => p.Realm, cellInfo.Realm);
                    var result = serversRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell updated successfully");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);
            }
            return Response;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("delete_cellInfo/{id}")]
        public APIResponse DeleteCellInfo(string id)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                serversRepository.Delete(expression);
                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "WebSphere cell deleted successfully");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), exception.Message);
            }

            return Response;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <param name="serverImport"></param>
        /// <returns></returns>
        [HttpPut("save_websphere_servers")]
        public APIResponse SaveWebSphereServers([FromBody]WebShpereServerImport serverImport)
        {

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);

                foreach (var serverModel in serverImport.SelectedServers)
                {
                    //if (serverModel.IsSelected)
                    {
                        //check to see if the node exists
                        List<Server> nodes = serversRepository.Find(x => x.Id == serverModel.NodeId).ToList();
                        if (nodes.Count > 0)
                        {
                            //node exists. Push the server id to the document
                            var updateDef = serversRepository.Updater.AddToSet(x => x.ServerId, serverModel.ServerId);
                            serversRepository.Update(nodes[0], updateDef);
                        }
                        else
                        {
                            Server node = new Server();
                            node.Id = serverModel.NodeId;
                            node.DeviceName = serverModel.NodeName;
                            node.DeviceType = Enums.ServerType.WebSphereNode.ToDescription();
                            node.IPAddress = serverModel.HostName;
                            node.CellId = serverModel.CellId;
                            node.ServerId = new string[] { serverModel.ServerId }.ToList();
                            serversRepository.Insert(node);
                        }                     


                        Server server = new Server();
                        server.Id = serverModel.ServerId;
                        server.CellId = serverModel.CellId;
                        server.NodeId = serverModel.NodeId;
                        server.DeviceName = serverModel.ServerName;
                        server.DeviceType = "WebSphere";
                        // server.LocationId = serverImport.Location;                       
                        serversRepository.Insert(server);

                        Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");

                        var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(server.Id));
                        foreach (var attribute in serverImport.DeviceAttributes)
                        {
                            if (!string.IsNullOrEmpty(attribute.FieldName))
                            {
                                string field = attribute.FieldName;
                                string value = attribute.DefaultValue;
                                string datatype = attribute.DataType;
                                if (datatype == "int")
                                {
                                    int outputvalue = Convert.ToInt32(value);
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                         .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "double")
                                {
                                    double outputvalue = Convert.ToDouble(value);
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                         .Set(field, outputvalue);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "bool")
                                {
                                    bool booloutput;
                                    if (value == "0")
                                    {
                                        booloutput = false;
                                    }
                                    else
                                    {
                                        booloutput = true;
                                    }
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, booloutput);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                                if (datatype == "string")
                                {
                                    UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                        .Set(field, value);
                                    var result = repository.Collection.UpdateMany(filter, updateDefinition);
                                }
                            }

                        }

                        // serversRepository.Insert(server);
                    }
                }
                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();
                Response = Common.CreateResponse("Success");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }
        #endregion
        #endregion

        #region Deleting Servers
        /// <summary>
        ///Delete Server
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete_server/{Id}")]
        public APIResponse DeleteServer(string Id)
        {
            try
            {
                dailyStatisticsRepository = new Repository<DailyStatistics>(ConnectionString);
                Expression<Func<DailyStatistics, bool>> dailyStatisticsExpression = (p => p.DeviceId == Id);
                dailyStatisticsRepository.Delete(dailyStatisticsExpression);
                databaseRepository = new Repository<Database>(ConnectionString);
                Expression<Func<Database, bool>> databaseExpression = (p => p.DeviceId == Id);
                databaseRepository.Delete(databaseExpression);
                ibmConnectionsObjectsRepository = new Repository<IbmConnectionsObjects>(ConnectionString);
                Expression<Func<IbmConnectionsObjects, bool>> ibmConnectionsObjectsExpression = (p => p.DeviceId == Id);
                ibmConnectionsObjectsRepository.Delete(ibmConnectionsObjectsExpression);
                mobiledevicesRepository = new Repository<MobileDevices>(ConnectionString);
                Expression<Func<MobileDevices, bool>> mobileDevicesExpression = (p => p.DeviceID == Id);
                mobiledevicesRepository.Delete(mobileDevicesExpression);
                sharepointWebTrafficDailyStatisticsRepository = new Repository<SharePointWebTrafficDailyStatistics>(ConnectionString);
                Expression<Func<SharePointWebTrafficDailyStatistics, bool>> sharepeointExpression = (p => p.DeviceId == Id);
                sharepointWebTrafficDailyStatisticsRepository.Delete(sharepeointExpression);
                statusRepository = new Repository<Status>(ConnectionString);
                Expression<Func<Status, bool>> statustExpression = (p => p.DeviceId == Id);
                statusRepository.Delete(statustExpression);

                statusDetailsRepository = new Repository<StatusDetails>(ConnectionString);
                Expression<Func<StatusDetails, bool>> statusDeatilstExpression = (p => p.DeviceId == Id);
                statusDetailsRepository.Delete(statusDeatilstExpression);
                summaryStatisticsRepository = new Repository<SummaryStatistics>(ConnectionString);
                Expression<Func<SummaryStatistics, bool>> summaryStatisticsExpression = (p => p.DeviceId == Id);
                summaryStatisticsRepository.Delete(summaryStatisticsExpression);
                travelerSummaryStatsRepository = new Repository<TravelerStatusSummary>(ConnectionString);
                Expression<Func<TravelerStatusSummary, bool>> travelerStatusSummaryExpression = (p => p.DeviceId == Id);
                travelerSummaryStatsRepository.Delete(travelerStatusSummaryExpression);
                //1/13/2017 NS commented out - there is no reference to a server in the business_hours collection
                //businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                //Expression<Func<BusinessHours, bool>> businessHoursExpression = (p => p.DeviceId == Id);
                //businessHoursRepository.Delete(businessHoursExpression);

                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> serverOtherExpression = (p => p.Id == Id);
                serverOtherRepository.Delete(serverOtherExpression);

                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> serverExpression = (p => p.Id == Id);
                serversRepository.Delete(serverExpression);

                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Server deleted sucessfully.");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server deletion has failed.\n Error Message :" + exception.Message);
            }

            return Response;
        }
        #endregion 

        #region Log Settings
        /// <summary>
        /// 
        /// </summary>
        /// <author>author</author>
        /// <returns></returns>
        [HttpGet("get_log_files")]
        public APIResponse GetLogFiles()
        {
            try
            {
                nameValueRepository = new Repository<NameValue>(ConnectionString);
                var loglevel = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Level").Select(x => x.Value).FirstOrDefault();
                var logfiles = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Files Path-New").Select(x => x.Value).FirstOrDefault();
                string[] filePaths = System.IO.Directory.GetFiles(logfiles);
                string[] folderPaths = System.IO.Directory.GetDirectories(logfiles);
                List<SendLogs> logfilenames = new List<SendLogs>();


                foreach (var x in filePaths)
                {

                    if (x.Contains("LogFiles.z"))
                        continue;
                    var path = x.Substring(x.LastIndexOf("\\") + 1);

                    SendLogs sendlogs = new SendLogs();
                    {
                        sendlogs.FileName = path.ToString();

                    }
                    logfilenames.Add(sendlogs);

                }
                List<string> sendlogfiles = new List<string>();
                List<ComboBoxListItem> combolist = new List<ComboBoxListItem>();
                //logs.LogFileName = sendlogfiles.ToList();

                foreach (var x in filePaths)
                {

                    if (x.Contains("LogFiles.z"))
                        continue;
                    var path = x.Substring(x.LastIndexOf("\\") + 1);


                    combolist.Add(new ComboBoxListItem { DisplayText = path });
                }


                Response = Common.CreateResponse(new { loglevel = loglevel, logfilenames = logfilenames, combolist = combolist });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Trying to navigate to the log file directory has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <author>author</author>
        /// <param name="logfile"></param>
        /// <returns></returns>
        [HttpGet("get_read_files/{logfile}")]
        public APIResponse GetreagLogFiles(string logfile)
        {
            try
            {
                var Text = "";
                nameValueRepository = new Repository<NameValue>(ConnectionString);

                var logfiles = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Files Path-New").Select(x => x.Value).FirstOrDefault();
                string filepath = logfiles + "\\" + logfile;
                double maxfileLength = 5;
                long length = new System.IO.FileInfo(filepath).Length;
                double lengthd = length / 1024 / 1024;
                if (lengthd >= maxfileLength)
                {
                    Text = "The log file you selected is too large to be displayed in the browser. Please view the file " + filepath + " directly on the server.";
                }
                else
                {
                    using (StreamReader streamReader = new StreamReader(filepath))
                    {
                        Text = streamReader.ReadToEnd();
                        streamReader.Close();
                    }
                }



                Response = Common.CreateResponse(Text);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Deletion of server credentials has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <author>Swathi</author>
        /// <param name="devicesettings"></param>
        /// <returns></returns>
        [HttpPut("save_log_settings")]
        public APIResponse UpdateLogSettings([FromBody] LogFolders devicesettings)
        {
            try
            {
                nameValueRepository = new Repository<NameValue>(ConnectionString);
                var logfiles = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Files Path-New").Select(x => x.Value).FirstOrDefault();
                string loglevel = Convert.ToString(devicesettings.LogLevel);
                string email = Convert.ToString(devicesettings.Email);
                
                var logslist = devicesettings;
                if(string.IsNullOrEmpty(email))
                {
                    var getloglevel = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Level").Select(x => x.Value).FirstOrDefault();
                    if (!string.IsNullOrEmpty(getloglevel))
                    {
                        var updateDefination = nameValueRepository.Updater.Set(p => p.Value, loglevel);
                        var filterDefination = Builders<NameValue>.Filter.Where(p => p.Name == "Log Level");

                        var update = nameValueRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(update, Common.ResponseStatus.Success.ToDescription(), "Log level updated successfully");

                    }
                    else
                    {
                        NameValue loglevels = new NameValue { Name = "Log Level", Value = loglevel };
                        string id = nameValueRepository.Insert(loglevels);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Log level inserted successfully");
                    }

                }

                else
                {
                    List<string> listoffiles = new List<string>();


                    foreach (var file in logslist.LogName)
                    {
                        string filename = file.ToString();
                        // filename = filename.Replace("{file_name:" , "");

                        string filepath = logfiles + "\\" + filename;
                        listoffiles.Add(filepath);
                    }
                    string[] paths = listoffiles.ToArray();

                    string[] oldZipFiles = System.IO.Directory.GetFiles(logfiles, "LogFiles.z*");
                    foreach (var files in oldZipFiles)
                        System.IO.File.Delete(files);
                    //Directory.Delete(files);

                    ZipFile zip = new ZipFile();

                    zip.AddFiles(listoffiles);
                    zip.MaxOutputSegmentSize = 3 * 1024 * 1024;
                    //zip.Save(logPath + "LogFiles.zip");
                    zip.Save(logfiles + "//LogFiles.zip");

                    //string[] zipFiles = System.IO.Directory.GetFiles(logPath, "LogFiles.z*");
                    string[] zipFiles = System.IO.Directory.GetFiles(logfiles, "LogFiles.z*");

                    var result = nameValueRepository.All()
                                         .Select(x => new
                                         {
                                             Name = x.Name,
                                             Value = x.Value
                                         }).ToList();

                    var host = result.Where(x => x.Name == "PrimaryHostName").Select(x => x.Value).FirstOrDefault();

                    var PEmail = result.Where(x => x.Name == "PrimaryUserId").Select(x => x.Value).FirstOrDefault();
                    var Ppwd = result.Where(x => x.Name == "Primarypwd").Select(x => x.Value).FirstOrDefault();
                    var port = result.Where(x => x.Name == "PrimaryPort").Select(x => x.Value).FirstOrDefault();

                    var auth = result.Where(x => x.Name == "PrimaryAuth").Select(x => x.Value).FirstOrDefault();
                    var PSSL = result.Where(x => x.Name == "PrimarySSL").Select(x => x.Value).FirstOrDefault();
                    bool gmail = host.ToUpper().Contains("GMAIL");


                    for (int i = 0; i < zipFiles.Length; i++)
                    {

                        string newfile = zipFiles[i];

                        MailMessage mail = new MailMessage();

                        System.Net.Mail.SmtpClient SmtpServer = new SmtpClient(host);
                        mail.From = new MailAddress(PEmail);
                        mail.To.Add(email);
                        mail.Subject = "Log Files";
                        mail.Body = "Log Files sent from VitalSigns.  File  " + (i + 1) + " of " + zipFiles.Length + ".";

                        System.Net.Mail.Attachment attachment;
                        attachment = new System.Net.Mail.Attachment(newfile);
                        mail.Attachments.Add(attachment);

                        SmtpServer.Port = Convert.ToInt32(port);
                        SmtpServer.Credentials = new System.Net.NetworkCredential(PEmail, Ppwd);
                        SmtpServer.EnableSsl = Convert.ToBoolean(PSSL);

                        SmtpServer.Send(mail);
                        Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Log Files Sent Succesfully");
                    }
                //  var logslist = ((Newtonsoft.Json.Linq.JObject)devicesettings.FileName).ToObject<List<LogFolders>>();
    
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving log scan servers has failed.\n Error Message :" + exception.Message);
            }
            return Response;

        }
        #endregion

        #region Add Servers
        /// <summary>
        ///Inserting Servers into Server Collection
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_servers")]
        public APIResponse UpdateServers([FromBody]ServersNewModel serverData)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);




                Server servers = new Server
                {
                    DeviceName = serverData.DeviceName,
                    DeviceType = serverData.DeviceType,
                    Description = serverData.Description,
                    LocationId = serverData.LocationId,
                    IPAddress = serverData.IpAddress,
                    BusinessHoursId = serverData.BusinessHoursId,
                    MonthlyOperatingCost = serverData.MonthlyOperatingCost,
                    IdealUserCount = serverData.IdealUserCount,
                    Category = serverData.Category
                };


                string id = serversRepository.Insert(servers);
                Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Server created successfully");

                Licensing licensing = new Licensing();
                licensing.refreshServerCollectionWrapper();

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server insert has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>

        [HttpPut("upload_file")]
        public APIResponse UploadFile()
        {
            try
            {
                string servers = "";
                string filePath = "uploads/";
                locationRepository = new Repository<Location>(ConnectionString);
                var locationList = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
                //we are passig only one file currently, but in case we do multiple, POC here:
                foreach (var fi in Request.Form.Files)
                {
                    System.IO.Stream f = fi.OpenReadStream();
                    System.Net.Mime.ContentDisposition c = new System.Net.Mime.ContentDisposition();
                    string fileName = fi.ContentDisposition.Substring(fi.ContentDisposition.IndexOf("filename=") + 10).Replace("\\", "");
                    fileName = fileName.Substring(0, fileName.Length - 1);
                    System.IO.FileStream fs = new System.IO.FileStream(filePath + fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);

                    f.CopyTo(fs);
                    fs.Dispose();

                    //read the file back if it's csv and process the file 
                    string logPath = filePath + fileName;
                    List<ServersModel> serverList = new List<ServersModel>();
                    if (fileName.ToLower().Contains(".csv"))
                    {
                        using (StreamReader sr = new StreamReader(logPath))
                        {
                            while (!sr.EndOfStream)
                            {
                                ServersModel server = new ServersModel();
                                server.DeviceName = sr.ReadLine();
                                server.DeviceType = "Domino";
                                server.IpAddress = "dummyaddress.yourdomain.com";
                                serverList.Add(server);
                            }
                            sr.Close();
                        }
                    }
                    //delete the file after done?
                    System.IO.File.Delete(logPath);
                    Response = Common.CreateResponse(new { locationList = locationList, serverList = serverList }, "OK", servers);
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
                return Response;
            }

        }
        #region Suspend Temporarly
        /// <summary>
        ///Suspended Server Temporarily
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        #region Suspend Temporarly


        [HttpPut("save_suspend_temporarly")]
        public APIResponse SaveSuspendTemporarly([FromBody]SuspendTemporarilyModel maintenance)
        {
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                Maintenance maintenancedata = new Maintenance
                {
                    Name = maintenance.Name + "-Temp-" + DateTime.Now.ToString(),
                    StartDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                    //StartTime = Convert.ToDateTime(DateTime.Now.ToShortTimeString()),
                    Duration = maintenance.Duration,
                    EndDate = Convert.ToDateTime(DateTime.Now.ToShortDateString()),
                    MaintenanceDaysList = "",
                    MaintainType = 1,

                };



                maintenance.Id = maintenanceRepository.Insert(maintenancedata);
                Response = Common.CreateResponse(maintenance.Id, Common.ResponseStatus.Success.ToDescription(), "Server scanning suspended for " + maintenance.Duration + " minutes.");

                serversRepository = new Repository<Server>(ConnectionString);
                UpdateDefinition<Server> updateDefinition = null;
                // var devicesList = ((Newtonsoft.Json.Linq.JArray)maintenance.DeviceList).ToObject<List<string>>();
                var serverId = maintenance.DeviceId;


                var server = serversRepository.Get(serverId);
                if (server.MaintenanceWindows != null)
                {
                    if (!server.MaintenanceWindows.Contains(maintenance.Id))
                    {
                        server.MaintenanceWindows.Add(maintenance.Id);
                        updateDefinition = serversRepository.Updater.Set(p => p.MaintenanceWindows, server.MaintenanceWindows);
                        var result = serversRepository.Update(server, updateDefinition);

                    }
                }
                else
                {
                    List<string> maintainanceWindow = new List<string>();
                    maintainanceWindow.Add(maintenance.Id);
                    updateDefinition = serversRepository.Updater.Set(p => p.MaintenanceWindows, maintainanceWindow);
                    var result = serversRepository.Update(server, updateDefinition);
                }




            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Temporarly suspending the server has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }
        #endregion
        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <author></author>
        /// <returns></returns>
        [HttpPut("upload_script")]
        public APIResponse uploadScripts()
        {
            string uploadedFile = "";
            try
            {
                string filePath = "uploads/";
                locationRepository = new Repository<Location>(ConnectionString);
                //we are passig only one file currently, but in case we do multiple, POC here:
                foreach (var fi in Request.Form.Files)
                {
                    System.IO.Stream f = fi.OpenReadStream();
                    System.Net.Mime.ContentDisposition c = new System.Net.Mime.ContentDisposition();
                    string fileName = fi.ContentDisposition.Substring(fi.ContentDisposition.IndexOf("filename=") + 10).Replace("\\", "");
                    fileName = fileName.Substring(0, fileName.Length - 1);
                    System.IO.Directory.CreateDirectory(filePath);
                    System.IO.FileStream fs = new System.IO.FileStream(filePath + fileName, System.IO.FileMode.Create, System.IO.FileAccess.Write);
                    //uploadedFile = "~/" + filePath + fileName;
                    uploadedFile = fs.Name;
                    f.CopyTo(fs);
                    fs.Dispose();
                    
                    //uploadedFile = System.Web.Hosting.HostingEnvironment.MapPath(filePath + fileName);
                    Response = Common.CreateResponse(uploadedFile, Common.ResponseStatus.Success.ToDescription(), "Script uploaded successfully");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), ex.Message);
            }
            return Response;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <author>Sowmya</author>
        /// <param name="device_id"></param>
        /// <returns></returns>
        #region Events

        [HttpGet("{device_id}/events")]
        public APIResponse GetEvents(string device_id)
        {
            try
            {
                List<ServerEventsModel> eventresult = new List<ServerEventsModel>();
                serversRepository = new Repository<Server>(ConnectionString);
                var server = serversRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == device_id);
                if (server != null)
                {
                    var severNotifications = server.NotificationList;
                    if (server.NotificationList != null)
                    {
                        foreach (var notificationId in severNotifications)
                        {
                            ServerEventsModel eventModel = new ServerEventsModel();
                            eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);
                            var eventMaster = eventsMasterRepository.Collection.Find(x => x.NotificationList.Contains(notificationId) && x.DeviceType == server.DeviceType).ToList();
                            if (eventMaster != null)
                            {
                                foreach (var eventm in eventMaster)
                                {
                                    eventModel.EventType += eventm.EventType + ",";
                                }
                                eventModel.EventType = eventModel.EventType.Substring(0, eventModel.EventType.Length - 1);
                            }
                            notificationsRepository = new Repository<Notifications>(ConnectionString);
                            var notifiname = notificationsRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == notificationId);
                            if (notifiname != null)
                            {
                                eventModel.NotificationName = notifiname.NotificationName;
                                foreach (var nofityDestinationId in notifiname.SendList)
                                {
                                    notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                                    var destnames = notificationDestRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == nofityDestinationId);
                                    if (!string.IsNullOrEmpty(destnames.BusinessHoursId))
                                    {
                                        businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                                        var business = businessHoursRepository.Collection.AsQueryable().FirstOrDefault(x => x.Id == destnames.BusinessHoursId);
                                        if (business != null)
                                        {
                                            eventModel.SendTo = destnames.SendTo;
                                            eventModel.CopyTo = destnames.CopyTo;
                                            eventModel.BlindCopyTo = destnames.BlindCopyTo;
                                            eventModel.StartTime = business.StartTime;
                                            eventModel.Duration = business.Duration;
                                            eventModel.Days = business.Days;
                                        }
                                    }
                                }
                                eventresult.Add(eventModel);
                            }
                        }
                    }
                }
                Response = Common.CreateResponse(eventresult);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting server events has failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion
        #region Scan Now
        /// <summary>
        ///Suspended Server Temporarily
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>

        [HttpPut("save_scan_now/{id}")]
        public APIResponse SaveScanNow(string id)
        {
            try
            {
                FilterDefinition<Status> statusFilterDefination = Builders<Status>.Filter.Where(p => p.DeviceId == id);
                statusRepository = new Repository<Status>(ConnectionString);

                var temp = statusRepository.Find(statusFilterDefination).ToList();
                var statusUpdateDefination = statusRepository.Updater.Set(p => p.Description, "Queued for immediate scanning...")
                                                                     .Set(p => p.Details, "Queued for immediate scanning...");

                var statusResult = statusRepository.Update(statusFilterDefination, statusUpdateDefination);

                // Response = Common.CreateResponse(statusResult, Common.ResponseStatus.Success.ToDescription(), "Server scan now successfully.");

                FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                serversRepository = new Repository<Server>(ConnectionString);


                var updateDefination = serversRepository.Updater.Set(p => p.ScanNow, true);

                var result = serversRepository.Update(filterDefination, updateDefination);

                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Server scan queued up successfully");




            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server scan now queue has failed.\n Error Message :" + exception.Message);
            }

            return Response;

        }

        #endregion

    }
}



