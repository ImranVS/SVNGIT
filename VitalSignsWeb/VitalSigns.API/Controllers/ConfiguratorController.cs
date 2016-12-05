using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using VitalSigns.API.Models.Configurator;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using VSNext.Mongo.Repository;
using VSNext.Mongo.Entities;
using System.Linq.Expressions;
using System.Linq;
using MongoDB.Bson;
using System.Runtime.Serialization.Json;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using System.Net.Mail;
using Ionic.Zip;

namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class ConfiguratorController : BaseController
    {
        #region Repository declaration
        private IRepository<BusinessHours> businessHoursRepository;
        private IRepository<Credentials> credentialsRepository;
        private IRepository<Location> locationRepository;

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
        private IRepository<EventsDetected> eventsdetectedRepository;

        private IRepository<Status> statusRepository;
        private IRepository<Nodes> nodesRepository;

        private IRepository<ServerOther> serverOtherRepository;
        private IRepository<EventsMaster> eventsMasterRepository;
        private IRepository<MobileDevices> mobileDevicesRepository;
        private IRepository<Notifications> notificationsRepository;
        private IRepository<NotificationDestinations> notificationDestRepository;
       private IRepository<Scripts> scriptsRepository;
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
                                                                new NameValue { Name = "Bing Key", Value = userpreference.BingKey }
                                                             };
                var result = Common.SaveNameValues(preferencesSettings);
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), " Settings were successully updated.");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Settings falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }
        [HttpGet("save_licence/{licencekey}")]
        public APIResponse SaveLicence(string licencekey)
        {
            try
            {
                var preferencesSettings = new List<NameValue> { new NameValue { Name = "Licence Key", Value = licencekey}};
                var result = Common.SaveNameValues(preferencesSettings);
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(),"Licence Key Saved Successfully Saved");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Licence Key falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }
        [HttpGet("get_preferences")]
        public APIResponse GetPreferences()
        {
            try
            {
                var preferencesSettings = new List<string> { "Company Name", "Currency Symbol", "Monitoring Delay", "Threshold Show", "Dashboard Only", "Bing Key" };
                PreferencesModel userpreference = new PreferencesModel();              
                var result = Common.GetNameValues(preferencesSettings);
                userpreference.CompanyName = result.FirstOrDefault(x => x.Name == "Company Name").Value;
                userpreference.CurrencySymbol = result.FirstOrDefault(x => x.Name == "Currency Symbol").Value;
                userpreference.MonitoringDelay =Convert.ToInt32(result.FirstOrDefault(x => x.Name == "Monitoring Delay").Value);
                userpreference.ThresholdShow = Convert.ToInt32(result.FirstOrDefault(x => x.Name == "Threshold Show").Value);
                userpreference.DashboardonlyExecSummaryButtons = Convert.ToBoolean(result.FirstOrDefault(x => x.Name == "Dashboard Only").Value);
                userpreference.BingKey = result.FirstOrDefault(x => x.Name == "Bing Key").Value;

                Response = Common.CreateResponse(userpreference);
               
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", " preferences falied .\n Error Message :" + exception.Message);
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
                   Alias=x.Alias,
                   UserId=x.UserId,
                   DeviceType=x.DeviceType,
                   Id=x.Id
                
                  

                }).ToList();
             

                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting Server Credentials falied .\n Error Message :" + exception.Message);
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
                if (string.IsNullOrEmpty(serverCredential.Id))
                {
                     filterExpression = (p => p.Alias == serverCredential.Alias);

                   
                }
                else
                {
                 filterExpression = (p => p.Alias == serverCredential.Alias && p.Id != serverCredential.Id);

                   
                }
                var existedData = credentialsRepository.Find(filterExpression).Select(x => x.Alias).FirstOrDefault();
                if (existedData==null)
                {
                    byte[] password;

                    password = tripleDes.Encrypt(serverCredential.Password);

                    System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                    foreach (byte b in password)
                    {
                        stringBuilder.AppendFormat("{0}, ", b);
                    }
                    string bytepwd = stringBuilder.ToString();
                    int n = bytepwd.LastIndexOf(", ");
                    bytepwd = bytepwd.Substring(0, n);
                    if (string.IsNullOrEmpty(serverCredential.Id))
                    {
                        Credentials serverCredentials = new Credentials { Alias = serverCredential.Alias, Password = bytepwd, DeviceType = serverCredential.DeviceType, UserId = serverCredential.UserId };


                        string id = credentialsRepository.Insert(serverCredentials);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Server Credential inserted successfully");
                    }
                    else
                    {

                        if (!serverCredential.IsModified)
                        {
                            FilterDefinition<Credentials> filterDefination = Builders<Credentials>.Filter.Where(p => p.Id == serverCredential.Id);
                            var updateDefination = credentialsRepository.Updater.Set(p => p.Alias, serverCredential.Alias)
                                                               .Set(p => p.DeviceType, serverCredential.DeviceType)

                                                               .Set(p => p.UserId, serverCredential.UserId);
                            var result = credentialsRepository.Update(filterDefination, updateDefination);
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Server Credential updated successfully");
                        }
                        else
                        {
                            FilterDefinition<Credentials> filterDefination = Builders<Credentials>.Filter.Where(p => p.Id == serverCredential.Id);
                            var updateDefination = credentialsRepository.Updater.Set(p => p.Alias, serverCredential.Alias)
                                                                     .Set(p => p.DeviceType, serverCredential.DeviceType)
                                                                     .Set(p => p.Password, bytepwd)
                                                                     .Set(p => p.UserId, serverCredential.UserId);
                            var result = credentialsRepository.Update(filterDefination, updateDefination);
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Server Credential updated successfully");
                        }

                    }
                }
             else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "This Alias name already exists.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Server Credentials falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Deleted Server Credential Suvcessfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server Credential delete falied .\n Error Message :" + exception.Message);
            }
            return Response;
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
                    var countryData = validLocationsRepository.All().Where(x => x.Country != null).Select(x => x.Country).Distinct().OrderBy(x => x).ToList();
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
                    locationRepository.Insert(location);
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Location inserted successfully");
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
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save locations falied .\n Error Message :" + exception.Message);
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
                Expression<Func<Location, bool>> expression = (p => p.Id == id);
              locationRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Location Deleted successfully");
            }
            catch(Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete locations falied .\n Error Message :" + exception.Message);
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
        public APIResponse GetAllBusinessHours(bool nameonly = false)
        {
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
                    Response = Common.CreateResponse(result);
                }
                else
                {
                    var result = businessHoursRepository.Find(_ => true)
                        .OrderBy(y => y.Name)
                        .Select(x => x.Name)
                        .ToList();
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching business hours failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        ///saves the  business hours data
        /// </summary>
        /// <author>Sowjanya</author>
       
        [HttpPut("save_business_hours")]
        public APIResponse UpdateBusinessHours([FromBody]BusinessHourModel businesshour)
        {
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
                     filterExpression = (p => p.Name == businesshour.Name && p.Id != businesshour.Id );
                    
                }
                var existsData = businessHoursRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();

                if (string.IsNullOrEmpty(existsData))
                {

                    if (string.IsNullOrEmpty(businesshour.Id))
                    {
                        BusinessHours businessHours = new BusinessHours { Name = businesshour.Name, StartTime = businesshour.StartTime, Duration = businesshour.Duration, Days = days.ToArray(), UseType = Convert.ToInt32(businesshour.UseType) };
                        string id = businessHoursRepository.Insert(businessHours);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Business hours inserted successfully");
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
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Business hours updated successfully");
                    }
                }

                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Name already exists. Enter another one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save business hours failed .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Business Hours failed .\n Error Message :" + exception.Message);
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
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                var maintainWindows = maintenanceRepository.All().Select(x => new MaintenanceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    StartTime = x.StartTime,
                    EndDate = x.EndDate,
                    Duration = x.Duration,
                    DurationType = Convert.ToString(x.DurationType),

                    MaintenanceDaysList = x.MaintenanceDaysList,
                    ContinueForever = x.ContinueForever,
                    MaintainType = Convert.ToString(x.MaintainType )== "1" ? "OneTime" :
                                   Convert.ToString(x.MaintainType) == "2" ? "Daily" :
                                    Convert.ToString(x.MaintainType) == "3" ? "Weekly" :
                                     Convert.ToString(x.MaintainType) == "4" ? "Monthly" : "-",

                    MaintainTypeValue = Convert.ToString(x.MaintainType)

                }).ToList();

                serversRepository = new Repository<Server>(ConnectionString);
                var servers = serversRepository.Collection.AsQueryable().Select(x => new { ServerID = x.Id, MaintenanceWindows = x.MaintenanceWindows });
                foreach (var maintainWindow in maintainWindows)
                {
                    var innerServers = servers.Where(x => x.MaintenanceWindows.Contains(maintainWindow.Id)).ToList();
                    foreach (var server in innerServers)
                    {
                        maintainWindow.DeviceList.Add(server.ServerID);
                    }
                }

                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);
                var keyUsers = mobileDevicesRepository.Collection.AsQueryable().Select(x => new { KeyuserID = x.Id, MaintenanceWindows = x.MaintenanceWindows});
                foreach(var maintenaceWindow in maintainWindows)
                {
                    var innerkeyUsers = keyUsers.Where(x => x.MaintenanceWindows.Contains(maintenaceWindow.Id)).ToList();
                  foreach(var keyUser in innerkeyUsers)
                    {
                        maintenaceWindow.KeyUsers.Add(keyUser.KeyuserID);
                    }

                }

                Response = Common.CreateResponse(maintainWindows, Common.ResponseStatus.Success.ToDescription(), "Maintenancedata inserted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching Maintenance failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        ///saves the  maintenance data
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpPut("save_maintenancedata")]
        public APIResponse UpdateMaintenancedata([FromBody]MaintenanceModel maintenance)
        {
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
                            MaintenanceDaysList = maintenance.MaintenanceDaysList,
                            MaintainType = maintenance.MaintainType == "" ? 0 : Convert.ToInt32(maintenance.MaintainType),
                            DurationType = maintenance.DurationType == "" ? 0 : Convert.ToInt32(maintenance.DurationType)
                        };


                        maintenance.Id = maintenanceRepository.Insert(maintenancedata);
                        Response = Common.CreateResponse(maintenance.Id, Common.ResponseStatus.Success.ToDescription(), "Maintenance data inserted successfully");


                    }
                    else
                    {
                        FilterDefinition<Maintenance> filterDefination = Builders<Maintenance>.Filter.Where(p => p.Id == maintenance.Id);
                        var updateDefination = maintenanceRepository.Updater.Set(p => p.Name, maintenance.Name)
                                                                 .Set(p => p.StartDate, maintenance.StartDate)
                                                                 .Set(p => p.StartTime, maintenance.StartTime)
                                                                 .Set(p => p.Duration, maintenance.Duration)
                                                                 .Set(p => p.EndDate, maintenance.EndDate)
                                                                  .Set(p => p.MaintainType, maintenance.MaintainType == "" ? 0 : Convert.ToInt32(maintenance.MaintainType))
                                                                  .Set(p => p.DurationType, maintenance.DurationType == "" ? 0 : Convert.ToInt32(maintenance.DurationType))
                                                                 .Set(p => p.MaintenanceDaysList, maintenance.MaintenanceDaysList);
                        var result = maintenanceRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Maintenance data  updated successfully");
                    }
         
                    serversRepository = new Repository<Server>(ConnectionString);
                    UpdateDefinition<Server> updateDefinition = null;
                     // var devicesList = ((Newtonsoft.Json.Linq.JArray)maintenance.DeviceList).ToObject<List<string>>();
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


                //var keyusersList = ((Newtonsoft.Json.Linq.JArray)maintenance.KeyUsers).ToObject<string[]>();
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
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Name  already exists. Enter another one.");
                }

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Maintenance data failed .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "Maintenance data deleted succesfully.");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Maintenance data falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        /// <summary>
        ///get the maintenance data for configurator
        /// </summary>
        /// <author>Sowjanya</author>
        [HttpGet("get_server_maintenancedata/{id}")]
        public APIResponse GetServerMaintenanceData(string id)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            maintenanceRepository = new Repository<Maintenance>(ConnectionString);
            List<MaintenanceModel> maintenanceWindows = new List<MaintenanceModel>();
            try
            {

                Expression<Func<Server, bool>> attributeexpression = (p => p.Id == id);

                var result = serversRepository.Find(attributeexpression).Select(x => x.MaintenanceWindows).FirstOrDefault();
                var results = maintenanceRepository.Collection.AsQueryable().Where(s=> result.Contains(s.Id))
                                                              .Select(m => new
                                                              {
                                                                  id = m.Id,
                                                                  Name = m.Name,
                                                                  StartDate = m.StartDate,
                                                                  StartTime = m.StartTime,
                                                                  EndDate = m.EndDate,
                                                                  Duration = m.Duration,
                                                                   MaintainType = m.MaintainType,
                                                                  MaintenanceFrequency = m.MaintenanceFrequency
                                                              }).ToList();
                Response = Common.CreateResponse(results);
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
                    LoginName = x.LoginName,
                    FullName = x.FullName,
                    Email = x.Email,
                    Status = x.Status,
                    SuperAdmin = x.SuperAdmin,
                    ConfiguratorAccess = x.ConfiguratorAccess,
                    ConsoleCommandAccess = x.ConsoleCommandAccess

                }).ToList();
                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get maintain users falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_maintain_users")]
        public APIResponse UpdateMaintainUsers([FromBody]MaintainUsersModel maintainuser)
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);
                Expression<Func<Users, bool>> filterExpression;
                if (string.IsNullOrEmpty(maintainuser.Id))
                {
                    filterExpression = (p => p.LoginName == maintainuser.LoginName);

                }
                else
                {
                    filterExpression = (p => p.LoginName == maintainuser.LoginName && p.Id != maintainuser.Id);

                }
                var existsData = maintainUsersRepository.Find(filterExpression).Select(x => x.LoginName).FirstOrDefault();
                if (string.IsNullOrEmpty(existsData))
                {
                    if (string.IsNullOrEmpty(maintainuser.Id))
                    {
                        Users maintainUsers = new Users { LoginName = maintainuser.LoginName, FullName = maintainuser.FullName, Email = maintainuser.Email, Status = maintainuser.Status, SuperAdmin = maintainuser.SuperAdmin, ConfiguratorAccess = maintainuser.ConfiguratorAccess, ConsoleCommandAccess = maintainuser.ConsoleCommandAccess };
                        string id = maintainUsersRepository.Insert(maintainUsers);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Maintain Users inserted successfully");
                    }
                    else
                    {
                        FilterDefinition<Users> filterDefination = Builders<Users>.Filter.Where(p => p.Id == maintainuser.Id);
                        var updateDefination = maintainUsersRepository.Updater.Set(p => p.LoginName, maintainuser.LoginName)
                                                                 .Set(p => p.FullName, maintainuser.FullName)
                                                                 .Set(p => p.Email, maintainuser.Email)
                                                                 .Set(p => p.Status, maintainuser.Status)
                                                                 .Set(p => p.SuperAdmin, maintainuser.SuperAdmin)
                                                                 .Set(p => p.ConfiguratorAccess, maintainuser.ConfiguratorAccess)
                                                                 .Set(p => p.ConsoleCommandAccess, maintainuser.ConsoleCommandAccess);

                        var result = maintainUsersRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Maintain Users updated successfully");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This " + maintainuser.LoginName  + "already exists. Enter another one.");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Maintain Users falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_maintain_users/{id}")]
        public APIResponse DeleteMaintainUsers(string id)
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);
                Expression<Func<Users, bool>> expression = (p => p.Id == id);
                maintainUsersRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(),"Maintain User Deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Maintain Users falied .\n Error Message :" + exception.Message);
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
                deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
                var result = deviceAttributesRepository.All().Where(x=>x.DeviceType==type).Select(x => new DeviceAttributesModel
                {
                    Id = x.Id,
                    AttributeName = x.AttributeName,
                    DefaultValue = x.DefaultValue,
                    DeviceType = x.DeviceType,
                    FieldName = x.FieldName,
                    DataType = x.DataType,
                    Unitofmeasurement = x.Unitofmeasurement,
                    IsSelected = false
                }).ToList();
                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get device attributes failed .\n Error Message :" + exception.Message);
            }
            return Response;
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
                if (deviceAttributes.Count() ==0)
                {
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Error.ToDescription(), "Please select atleast one Attribute.");
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
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server Attributes Updated Successfully.");
                }
                    // var result = repository.Collection.UpdateMany(filter, updateDefinition);
                    
                
            
                    


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server Attributes Updation failed .\n Error Message :" + exception.Message);
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



                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get server tasks failed .\n Error Message :" + exception.Message);
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

               
                if (devicesList.Count() > 0 && selectedServerTasks.Count()>0 && !string.IsNullOrEmpty(setting.Trim()))
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
                                Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Task Name  name already exists. Enter another one.");
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
                                    Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Domino Server Tasks added Successfully.");
                                }
                            }
                             if (setting.Equals("remove"))
                            {
                                var dominoServerTaskRemove = dominoServerTasks.Where(x => x.TaskId == serverTask.Id).ToList();
                                foreach(var item in dominoServerTaskRemove)
                                dominoServerTasks.Remove(item);
                                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Domino Server Tasks deleted Successfully.");
                            }
                             
                        }
                       

                     
                            updateDefinition = serversRepository.Updater.Set(p => p.ServerTasks, dominoServerTasks);
                            var result = serversRepository.Update(server, updateDefinition);
                        

                    }
                   // Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Settings are not selected");


                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Devices were not selected");
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Domino Server Tasks falied .\n Error Message :" + exception.Message);
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
        public APIResponse GetAllWindowservices()
        {
            try
            {
                windowsservicesRepository = new Repository<WindowsService>(ConnectionString);
                var result = windowsservicesRepository.All().Select(x => new WindowsServiceModel
                {
                   // Id = x.Id,
                    ServiceName = x.ServiceName,
                    IsSelected = false
                    // Id = x.Id


                }).ToList();


                Response = Common.CreateResponse(result);

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
                                    diskSettings.Add(new DiskSetting { DiskName = item.DiskName, Threshold = Convert.ToDouble(item.FreespaceThreshold), ThresholdType = item.ThresholdType });
                                }
                            }
                        }
                        if (diskSettings.Count > 0)
                        {
                            updateDefinition = serversRepository.Updater.Set(p => p.DiskInfo, diskSettings);
                            var result = serversRepository.Update(server, updateDefinition);
                        }                      
                   
                    }
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Disk Settings Sucessfully updated.");


                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Devices were not selected");
                }

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Server Disk Settings failed" + exception.Message);
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
                var serverTypeData = serverTypeRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Name }).ToList().OrderBy(x => x.DisplayText);
               // var credentialServerTypeData = serverTypeRepository.All().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.ServerTypeId.ToString() }).ToList().OrderBy(x => x.DisplayText);
                var credentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var businessHoursData = businessHoursRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var locationsData = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                Response = Common.CreateResponse(new { credentialsData = credentialsData, businessHoursData = businessHoursData, locationsData = locationsData, serverTypeData= serverTypeData });
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
                            Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Settings updated successfully");
                        }
                        else
                        {
                            Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Settings are not selected");
                        }
                    }

                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Devices were not selected");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Settings Failed .\n Error Message :" + exception.Message);
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
                    IsEnabled=s.IsEnabled,
                    Platform=s.Platform,
                    LocationId = s.LocationId,
                    Devicetype = s.DeviceType,
                    CellId = s.CellId,
                    NodeId = s.NodeId

                    // CellName = serversRepository.Collection.Find(filter).AsQueryable().Select(x => x.NodeId).FirstOrDefault(),
                    //   NodeName = serversRepository.Collection.AsQueryable().Select(x => x.NodeIds).FirstOrDefault()


                }).FirstOrDefault();
                var locationname = locationRepository.All().Where(x => x.Id == serverresult.LocationId).Select(x => new Location
                {
                    LocationName = x.LocationName


                }).FirstOrDefault();
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
                serverresult.LocationId = locationname.LocationName;
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
                        if (serverValues.Contains(attri.FieldName) )
                        {
                       
                            var servervalue = serverValues.Where(x => x.Name == attri.FieldName).Select(x => x.Value).FirstOrDefault();
                        attri.DefaultValue = servervalue.ToString();
                        //if (attri.DataType == "bool" && attri.DefaultValue == "false")
                        //{
                        //    attri.DefaultValue = "0";
                        //}
                        //else
                        //{
                        //    attri.DefaultValue = "1";
                        //}

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

                       
                        serverresult.DeviceAttributes.Add(attri);
                    

                    }
                }
                   
               

                //Response = Common.CreateResponse(serverresult);
                Response = Common.CreateResponse(new { credentialsData = credentialsData, serverresult = serverresult,platform=serverresult.Platform });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching Server Atributes failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///saves the Server device attributes data
        /// </summary>
        /// <author>Swathi</author>
        [HttpPut("save_servers_attributes/{id}")]
        public APIResponse SaveServerDeviceAttributes([FromBody]DeviceSettings serverAttributes,string id)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                Repository repository = new Repository(Startup.ConnectionString, Startup.DataBaseName, "server");
                var deviceAttributes = ((Newtonsoft.Json.Linq.JObject)serverAttributes.Value).ToObject<DeviceAttributesDataModel>();
                var filter = Builders<BsonDocument>.Filter.Eq("_id", ObjectId.Parse(id));
                var filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                var locationname = locationRepository.All().Where(x => x.LocationName == deviceAttributes.LocationId).Select(x => new Location
                {
                    Id = x.Id


                }).FirstOrDefault();
                deviceAttributes.LocationId = locationname.Id;
                //   UpdateDefinition<BsonDocument> updateserverDefinition = Builders<BsonDocument>.Update.Set(devicename=devicename,, category, ipaddress, isenabled, location, description);
                //  var serverresult = repository.Collection.UpdateMany(filter, updateserverDefinition);
                var updateDefination = serversRepository.Updater.Set(p => p.DeviceName, deviceAttributes.DeviceName)
                                                                  .Set(p => p.Category, deviceAttributes.Category)
                                                                  .Set(p => p.IPAddress, deviceAttributes.IPAddress)
                                                                  .Set(p => p.LocationId, deviceAttributes.LocationId)
                                                                  .Set(p => p.Description, deviceAttributes.Description)
                                                                  .Set(p => p.IsEnabled, deviceAttributes.IsEnabled)
                                                                  .Set(p => p.RequireSSL, deviceAttributes.RequireSSL)
                                                                  .Set(p => p.CredentialsId, deviceAttributes.CredentialsId);


                var serverresult = serversRepository.Update(filterDefination, updateDefination);

                if (deviceAttributes.DeviceAttributes.Count() > 0)
                {
                    foreach (var attribute in deviceAttributes.DeviceAttributes)
                    {
                        if (!string.IsNullOrEmpty(attribute.FieldName))
                        {
                            string field = attribute.FieldName;
                            string value = attribute.DefaultValue;
                            string datatype = attribute.DataType;
                            if(field == "password")
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
                            if(datatype=="int")
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
                                if (value=="False" || value=="0")
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


                            if (datatype=="string")
                            {
                                UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                                                                                    .Set(field, value);
                                var result = repository.Collection.UpdateMany(filter, updateDefinition);
                            }
                            if (datatype == "ObjectId")
                            {
                                UpdateDefinition<BsonDocument> updateDefinition = Builders<BsonDocument>.Update
                                                                                                    .Set(field, ObjectId.Parse(value));
                                var result = repository.Collection.UpdateMany(filter, updateDefinition);
                            }

                        }                       

                    }
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server Attributes data Updated Successfully.");   
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Server Attributes falied .\n Error Message :" + exception.Message);
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

                        PercentFree = drive.PercentFree,
                      
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
                            drives.FirstOrDefault(x => x.DiskName == item.DiskName).IsSelected = true;

                        }
                    }
                }
                Response = Common.CreateResponse(drives);
            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error in getting disk information");
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
                   var results=result.Select(s=>new SelectedDiksModel {DiskName =(s.DiskName == "AllDisks" && s.ThresholdType == "GB" ? "allDisksByGB" :
                                                                      s.DiskName == "AllDisks" && s.ThresholdType == "Percent" ? "allDisksBypercentage" :
                                                                      s.DiskName == "NoAlerts" ? "noDiskAlerts" :
                                                                      "selectedDisks"),
                                                                      ThresholdType = s.ThresholdType,
                                                                      FreespaceThreshold = s.Threshold.ToString()

                }).FirstOrDefault();
                    Response = Common.CreateResponse(results);
                }
             

               

               

            }

            
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get server disk settings data falied.\n Error Message :" + exception.Message);
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
                                diskSettings.Add(new DiskSetting { DiskName = item.DiskName, Threshold = Convert.ToDouble(item.FreespaceThreshold), ThresholdType = item.ThresholdType });
                            }
                        }
                    }
                    if (diskSettings.Count > 0)
                    {
                        updateDefinition = serversRepository.Updater.Set(p => p.DiskInfo, diskSettings);
                        var result = serversRepository.Update(server, updateDefinition);
                    }

            
                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Server Disk Settings updated successfully.");

            }
              
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Servers were not selected");
                }

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Server disk settings failed.\n Error Message :" + exception.Message);
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

                Response = Common.CreateResponse(null, "Error", "Error in getting  server tasks  data ");
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
                Response = Common.CreateResponse(new { TaskNames = result1});
               
            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error in getting task names");
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
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This  Task Name   already exists. Enter another one.");
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
                        Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Domino Server Tasks inserted Successfully");

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
                        Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Domino Server Tasks updated Successfully");
                    }



                    var updateDefinitaion = serversRepository.Updater.Set(p => p.ServerTasks, serverTasks);
                    var filterDefination = Builders<Server>.Filter.Where(p => p.Id == servertasks.DeviceId);
                    serversRepository.Update(filterDefination, updateDefinitaion);

                }

                
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Domino Server Tasks  falied .\n Error Message :" + exception.Message);
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

                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Server Tasks deleted Successfully.");
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Server Tasks falied .\n Error Message :" + exception.Message);
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

            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                var results = serversRepository.Collection.AsQueryable().Where(x => x.Id == id)
                            .Select(x => new AdvancedSettingsModel
                            {
                                MemoryThreshold =x.MemoryThreshold,
                                CpuThreshold = x.CpuThreshold,
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
                                DatabaseSettingsPort=x.DatabaseSettingsPort,
                                DeviceType = x.DeviceType,
                                CollectConferenceStatistics=x.CollectConferenceStatistics,
                                ClusterReplicationQueueThreshold=x.ClusterReplicationQueueThreshold,
                              
                                
                                

                            }).FirstOrDefault();

                //if (results.DeviceType == "IBM Connections")
                //{
                //    var ibmCredentialname = credentialsRepository.All().Where(x => x.Id == results.DatabaseSettingsCredentialsId).Select(x => new Credentials
                //    {
                //        Alias = x.Alias


                //    }).FirstOrDefault();
                //    results.DatabaseSettingsCredentialsId = ibmCredentialname.Alias;
                //}
                Response = Common.CreateResponse(results);
            }
            catch (Exception ex)
            {

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Error in getting Advanced Settings.");
            }



            return Response;
        }
        /// <summary>
        /// Updates Advanced  Settings
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_advanced_settings/{id}")]
        public APIResponse UpdateAdvancedSettings([FromBody]AdvancedSettingsModel advancedSettings,string id)
        {
            try
            {
                FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);
                serversRepository = new Repository<Server>(ConnectionString);
               
                try
                {
                   if(advancedSettings.DeviceType=="Domino")
                    {
                       
                        var updateDefination = serversRepository.Updater.Set(p => p.MemoryThreshold, advancedSettings.MemoryThreshold)
                                                                 .Set(p => p.CpuThreshold, advancedSettings.CpuThreshold)
                                                                 .Set(p => p.ServerDaysAlert, advancedSettings.ServerDaysAlert)
                                                                 .Set(p => p.ClusterReplicationDelayThreshold, advancedSettings.ClusterReplicationDelayThreshold)
                                                                 .Set(p=> p.ClusterReplicationQueueThreshold,advancedSettings.ClusterReplicationQueueThreshold);
                        var result = serversRepository.Update(filterDefination, updateDefination);
                      //  Response = Common.CreateResponse(result);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advaced Settings updated successfully.");
                    }
                   else if(advancedSettings.DeviceType == "Sametime")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.ProxyServerType, advancedSettings.ProxyServerType)
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
                                                               .Set(p=>p.CollectConferenceStatistics,advancedSettings.CollectConferenceStatistics)
                                                               .Set(p=>p.Db2SettingsCredentialsId,advancedSettings.Db2SettingsCredentialsId);

                        var result = serversRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advaced Settings updated successfully.");
                        //Response = Common.CreateResponse(result);

                    }

                   else if (advancedSettings.DeviceType == "IBM Connections")
                    {
                        var updateDefination = serversRepository.Updater.Set(p => p.DatabaseSettingsHostName, advancedSettings.DatabaseSettingsHostName)
                                                             .Set(p => p.DatabaseSettingsPort, advancedSettings.DatabaseSettingsPort)
                                                             .Set(p=>p.DatabaseSettingsCredentialsId,advancedSettings.DatabaseSettingsCredentialsId);
                                                            

                        var result = serversRepository.Update(filterDefination, updateDefination);
                        //Response = Common.CreateResponse(result);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Advaced Settings updated successfully.");

                    }
                  
                   
                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Advanced Settings falied .\n Error Message :" + exception.Message);
                }

                return Response;


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Advanced Settings falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }
       
        #endregion
        #endregion

        #region IBM Domino Settings

        #region Custom Statistics
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
                    ConsoleCommand = x.ConsoleCommand

                }).ToList();
                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "custom statistics falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

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
                        TimesInARow  = customstat.TimesInARow,
                        GreaterThanOrLessThan = customstat . GreaterThanOrLessThan,
                        ConsoleCommand = customstat.ConsoleCommand
                    
                    };
                    string id = serverOtherRepository.Insert(customstatistic);
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "custom statistics inserted successfully");
                }
                else
                {
                    FilterDefinition<ServerOther> filterDefination = Builders<ServerOther>.Filter.Where(p => p.Id == customstat.Id);                 
                    var updateDefination = serverOtherRepository.Updater.Set(p => p.DominoServers, devicesList)
                                                             .Set(p => p.StatName, customstat.StatName)
                                                             .Set(p => p.ThresholdValue, customstat.ThresholdValue)
                                                             .Set(p => p.TimesInARow, customstat.TimesInARow)
                                                              .Set(p => p.GreaterThanOrLessThan, customstat.GreaterThanOrLessThan)
                                                               .Set(p => p.ConsoleCommand, customstat.ConsoleCommand);                                                            
                    var result = serverOtherRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "custom statistics updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save custom statistics falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpDelete("delete_custom_statistics/{Id}")]
        public APIResponse DeleteCustomStatistics(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Custom Statistics Deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete custom statistics falied .\n Error Message :" + exception.Message);
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
                var serversData = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType == "Domino").Select(x => new ComboBoxListItem { DisplayText = x.DeviceName, Value = x.DeviceName }).ToList().OrderBy(x => x.DisplayText);
                Response = Common.CreateResponse(new { serversData = serversData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching Domino servers failed .\n Error Message :" + exception.Message);
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
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type== "Notes Database Replica").Select(x => new NotesDatabaseReplicaModel
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


                }).ToList().OrderBy(x => x.Name);


                Response = Common.CreateResponse(result);
               // Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Delete Server Credentials falied .\n Error Message :");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting Notes Database Replica falied .\n Error Message :" + exception.Message);
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
                    filterExpression = (p => p.Name == notesDatabaseReplica.Name);


                }
                else
                {
                    filterExpression = (p => p.Name == notesDatabaseReplica.Name && p.Id != notesDatabaseReplica.Id);


                }
                var existedData = serverOtherRepository.Find(filterExpression).Select(x => x.Name).FirstOrDefault();
                if(existedData==null)
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
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Notes Database Replica inserted successfully.");
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
                                                                  .Set(p => p.DifferenceThreshold, notesDatabaseReplica.DifferenceThreshold)
                                                                 ;
                        var result = serverOtherRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Notes Database Replica updated successfully.");
                    }
                }
                else
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "This name already exists.");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes Database Replica update falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }

        [HttpDelete("notes_database_replica/{Id}")]
        public APIResponse DeleteNotesDatabaseReplica(string Id)
        {
            try
            {
                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> expression = (p => p.Id == Id);
                serverOtherRepository.Delete(expression);

                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Notes Database Replica Deleted Sucessfully.");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes Database Replic delete falied .\n Error Message :" + exception.Message);
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

                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Getting IBM Domino Settings falied .\n Error Message :" + exception.Message);
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
                        var password = new List<NameValue> {
                                                                new NameValue { Name = "Password", Value = bytepwd}
                                                             };
                        var passwordResult = Common.SaveNameValues(password);
                    }

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
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "IBM Domino settings are successfully updated.");

                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save IBM Domino Settings falied .\n Error Message :" + exception.Message);
                }

                return Response;


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save IBM Domino Settings falied .\n Error Message :" + exception.Message);
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
        public APIResponse GetAllNotesDatabases()
        {
            try
            {

                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                var result = serverOtherRepository.Collection.AsQueryable().Where(x => x.Type == "Notes Database").Select(x => new NotesDatabaseModel
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

                    TriggerValue = x.TriggerValue

                }).ToList().OrderBy(x => x.Name);

                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Delete Server Credentials falied .\n Error Message :" + exception.Message);
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
                    filterExpression = (p => p.Name == notesDatabase.Name);
                }
                else
                {
                    filterExpression = (p => p.Name == notesDatabase.Name && p.Id != notesDatabase.Id);

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
                            //DominoServerId = notesDatabase.DominoServerId
                        };


                        string id = serverOtherRepository.Insert(notesDatabases);
                        Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Notes Database inserted successfully");
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
                                                                 .Set(p => p.RetryInterval, notesDatabase.RetryInterval);

                        var result = serverOtherRepository.Update(filterDefination, updateDefination);
                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Notes Database updated successfully");
                    }



                }
                else
                {
                    Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Name already exists. Enter another one.");
                }
               
                    
               
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Notes Database falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(false, Common.ResponseStatus.Success.ToDescription(), "Notes databases data deleted succesfully.");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Notes Database failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        #endregion

        #region IBM Domino Server Tasks

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
                Response = Common.CreateResponse(null, "Error", "Get domino server task falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

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
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Domino Server Task falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_server_task_definition/{id}")]
        public APIResponse DeleteServerTaskDefinition(string id)
        {
            try
            {
                dominoservertasksRepository = new Repository<DominoServerTasks>(ConnectionString);
                Expression<Func<DominoServerTasks, bool>> expression = (p => p.Id == id);
                dominoservertasksRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Server Task Deleted successfully");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Server Task falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(null, "Error", "Get Log File Scanning falied .\n Error Message :" + exception.Message);
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
                if ( id!="-1")
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
                Response = Common.CreateResponse(null, "Error", "Get Event Log File Scanning falied .\n Error Message :" + exception.Message);
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
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "");
                }
                else
                {
                    serverOtherRepository = new Repository<ServerOther>(ConnectionString);

                    string settingValue = Convert.ToString(devicesettings.Value);

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
                                Response = Common.CreateResponse(newid, Common.ResponseStatus.Success.ToDescription(), "Domino Event Definition  inserted successfully");
                            }
                            if (id!=("-1"))
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
                                        Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Domino Event Log Scanning updated successfully");

                                    }

                                }
                            }
                        
                       
                    }
                    else if (existsData == settingValue)
                    {
                        Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "This Event Definition " + "" + existsData + " " + "already exists. Please Enter another one.");
                    }

                    if (logfiles.Count==0)
                    {
                        Response = Common.CreateResponse(false, Common.ResponseStatus.Error.ToDescription(), "Please create at least one Domino Event Log entry.");
                    }
                }
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Domino Event Definition falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Domino Event Definition Deleted successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Domino Event Definition falied .\n Error Message :" + exception.Message);
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

                var serverTaskDelete = dominoServerTasks.FirstOrDefault(x => x.EventId == id);
                if (serverTaskDelete != null)
                {
                    dominoServerTasks.Remove(serverTaskDelete);
                    var updateDefinition = serverOtherRepository.Updater.Set(p => p.LogFileKeywords, dominoServerTasks);
                    var result = serverOtherRepository.Update(server, updateDefinition);
                    Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Domino Event Log Scanning Deleted successfully");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Event Log File falied .\n Error Message :" + exception.Message);
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

                }).ToList();
                statusRepository = new Repository<Status>(ConnectionString);
                var travelerServers = statusRepository.Collection.AsQueryable().Where(x => x.SecondaryRole.Contains("Traveler")).Select(x => new ComboBoxListItem { DisplayText = x.DeviceName, Value = x.Id }).OrderBy(x => x.DisplayText).ToList();
                Response = Common.CreateResponse(new { travellerData = travellerData, travelerServers = travelerServers });
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get traveler data falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_traveler_data_store")]
        public APIResponse UpdateTravelerDataStore([FromBody]TravelerDataStoresModel travelerdatas)
        {
            try
            {
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                if (string.IsNullOrEmpty(travelerdatas.Id))
                {
                    TravelerDTS travelerds = new TravelerDTS { TravelerServicePoolName = travelerdatas.TravelerServicePoolName, DeviceName = travelerdatas.DeviceName, DataStore = travelerdatas.DataStore, DatabaseName = travelerdatas.DatabaseName, Port = travelerdatas.Port, UserName = travelerdatas.UserName, Password = travelerdatas.Password, IntegratedSecurity = travelerdatas.IntegratedSecurity, TestScanServer = travelerdatas.TestScanServer };//, UsedByServers = travelerdatas.UsedByServers };
                    string id = travelerdatastoreRepository.Insert(travelerds);
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "traveler data inserted successfully");
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
                                                             .Set(p => p.Password, travelerdatas.Password)
                                                             .Set(p => p.IntegratedSecurity, travelerdatas.IntegratedSecurity)
                                                             .Set(p => p.TestScanServer, travelerdatas.TestScanServer);
                    //.Set(p => p.UsedByServers, travelerdatas.UsedByServers);
                    var result = travelerdatastoreRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "traveler data updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save traveler data falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpDelete("delete_traveler_data_store/{id}")]
        public APIResponse DeleteTravelerDataStore(string id)
        {
            try
            {
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                Expression<Func<TravelerDTS, bool>> expression = (p => p.Id == id);
                travelerdatastoreRepository.Delete(expression);
                Response = Common.CreateResponse(true, Common.ResponseStatus.Success.ToDescription(), "Traveler Data Store Deleted successfully");
            }
            catch(Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Delete Traveler Data falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #endregion

        #region Alerts

        #region Alert Settings

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
                
                try
                {
                    var alertData = new List<NameValue> { new NameValue { Name = "PrimaryHostName", Value = alertSettings.PrimaryHostName },
                        new NameValue { Name = "PrimaryFrom", Value = alertSettings.PrimaryForm },
                        new NameValue { Name = "PrimaryUserId", Value = alertSettings.PrimaryUserId},
                        new NameValue { Name = "Primarypwd", Value = alertSettings.PrimaryPwd},
                        new NameValue { Name = "PrimaryPort", Value =Convert.ToString(alertSettings.PrimaryPort)},
                        new NameValue { Name = "SmsForm", Value =  alertSettings.SmsForm},
                        new NameValue { Name = "PrimaryAuth", Value =Convert.ToString(alertSettings.PrimaryAuth)},
                        new NameValue { Name = "PrimarySSL", Value = Convert.ToString(alertSettings.PrimarySSL)},
                        new NameValue { Name = "SecondaryHostName", Value =  Convert.ToString(alertSettings.SecondaryHostName)},
                        new NameValue { Name = "SecondaryFrom", Value = alertSettings.SecondaryForm},
                        new NameValue { Name = "SecondaryUserId", Value = alertSettings.SecondaryUserId},
                        new NameValue { Name = "SecondaryPwd", Value =  Convert.ToString(alertSettings.SecondaryPwd)},
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
                        new NameValue {Name = "NumberOfRecurrences", Value= alertSettings.NumberOfRecurrences.ToString()}
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

                    Response = Common.CreateResponse(true, "OK", "Alert Settings were successully updated.");
                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, "Error", "Saving Alert Settings failed .\n Error Message :" + exception.Message);
                }
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Saving Alert Settings failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

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
                AlertInterval = Convert.ToInt32(alertInterval),
                AlertDuration = Convert.ToInt32(alertDuration),
                //EMail = email,
                EnableAlertLimits = Convert.ToBoolean(enableAlertLimits),
                TotalMaximumAlertsPerDay = Convert.ToInt32(totalMaximumAlertsPerDay),
                TotalMaximumAlertsPerDefinition = Convert.ToInt32(totalMaximumAlertsPerDefinition),
                EnableSNMPTraps = Convert.ToBoolean(enableSNMPTraps),

                HostName = hostName,
                AlertAboutRecurrencesOnly = Convert.ToBoolean(alertAboutRecurrencesOnly),
                NumberOfRecurrences = Convert.ToInt32(numberOfRecurrences)

            });
        }

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
        [HttpGet("viewalerts")]
        public APIResponse GetViewalerts(DateTime statdate)
        {
            try
            {
                DateTime? nullDate = null;
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                if (statdate == DateTime.MinValue || statdate.Date == DateTime.Now.Date)
                {
                    statdate = DateTime.Now;
                    var resultList = eventsdetectedRepository.All().Where(x => x.EventDetected.HasValue && x.EventDetected.Value.Date == statdate.Date).ToList();
                    var result = resultList                        
                        .Select(s => new AlertsModel
                        {
                            DeviceName = s.Device,
                            DeviceType = s.DeviceType,
                            AlertType = s.EventType,
                            Details = s.Details,
                            EventDetectedSent = s.NotificationsSent != null ? (s.NotificationsSent[s.NotificationsSent.Count-1].EventDetectedSent.Value) : nullDate,
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
                            DeviceName = s.Device,
                            DeviceType = s.DeviceType,
                            AlertType = s.EventType,
                            Details = s.Details,
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

        private List<dynamic> GetNotificationsList()
        {
            List<dynamic> result_disp = new List<dynamic>();
            List<dynamic> result_sendto = new List<dynamic>();
            List<dynamic> result_escalateto = new List<dynamic>();
            List<dynamic> result_events = new List<dynamic>();
            List<dynamic> result_servers = new List<dynamic>();
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

                serversRepository = new Repository<Server>(ConnectionString);
                notificationsRepository = new Repository<Notifications>(ConnectionString);
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                var notificationDestinations = notificationDestRepository.Collection.AsQueryable().ToList();
                var hoursname = "";
                foreach (var notificationDest in notificationDestinations)
                {
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
                    hour_ids.Add(notificationDest.Id.ToString());
                }

                var notifications = notificationsRepository.Collection.AsQueryable().ToList();

                var eventsList = eventsMasterRepository.Find(_ => true).OrderBy(x => x.DeviceType).ToList();
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
                }

                var serversList = serversRepository.Find(_ => true).OrderBy(x => x.DeviceName).ToList();
                foreach (var serverItem in serversList)
                {
                    result_servers.Add(new ServerDefinition
                    {
                        Id = serverItem.Id,
                        IsSelectedServer = false,
                        DeviceName = serverItem.DeviceName,
                        DeviceType = serverItem.DeviceType,
                        Location = serverItem.LocationId,
                        Description = serverItem.Description
                    });
                }

                foreach (var notification in notifications)
                {
                    is_selected_event = new List<bool>();
                    event_ids = new List<string>();
                    foreach (var eventItem in eventsList)
                    {
                        var is_sel = false;
                        if (eventItem.NotificationList != null)
                        {
                            var found = eventItem.NotificationList.Find(x => x == notification.Id);
                            if (found != null)
                            {
                                is_sel = true;
                            }
                        }
                        is_selected_event.Add(is_sel);
                        event_ids.Add(eventItem.Id);
                    }

                    is_selected_server = new List<bool>();
                    server_ids = new List<string>();
                    foreach (var serverItem in serversList)
                    {
                        var is_sel = false;
                        if (serverItem.NotificationList != null)
                        {
                            var found = serverItem.NotificationList.Find(x => x == notification.Id);
                            if (found != null)
                            {
                                is_sel = true;
                            }
                        }
                        is_selected_server.Add(is_sel);
                        server_ids.Add(serverItem.Id);
                    }

                    is_selected_hour = new List<bool>();
                    send_to = new List<string>();
                    send_via = new List<string>();
                    foreach (var notificationDest in notificationDestinations)
                    {
                        var is_sel = false;
                        foreach (var sendto in notification.SendList)
                        {
                            if (sendto == notificationDest.Id)
                            {
                                is_sel = true;
                                if (notificationDest.Interval == null)
                                {
                                    send_to.Add(notificationDest.SendTo);
                                    send_via.Add(notificationDest.SendVia);
                                }
                            }
                        }
                        is_selected_hour.Add(is_sel);

                    }

                    result_disp.Add(new NotificationsModel
                    {
                        ID = notification.Id.ToString(),
                        NotificationName = notification.NotificationName,
                        SendVia = String.Join(", ", send_via.ToArray()),
                        SendTo = String.Join(", ", send_to.ToArray()),
                        SendViaList = send_via,
                        SendToList = send_to,
                        BusinessHoursIds = hour_ids,
                        IsSelectedHour = is_selected_hour,
                        EventIds = event_ids,
                        IsSelectedEvent = is_selected_event,
                        ServerIds = server_ids,
                        IsSelectedServer = is_selected_server
                    });

                    //if (notificationDest.Interval == null)
                    //{

                    //}

                    //foreach (var sendto in notification.SendList)
                    //{
                    //    foreach (var notificationDest in notificationDestinations)
                    //    {
                    //        if (sendto == notificationDest.Id)
                    //        {
                    //            if (notificationDest.Interval == null)
                    //            {
                    //                result_disp.Add(new NotificationsModel
                    //                {
                    //                    ID = notification.Id.ToString(),
                    //                    NotificationName = notification.NotificationName,
                    //                    SendVia = String.Join(", ", send_via.ToArray()),
                    //                    SendTo = String.Join(", ", send_to.ToArray()),
                    //                    BusinessHoursIds = hour_ids,
                    //                    IsSelectedHour = is_selected_hour,
                    //                    EventIds = event_ids,
                    //                    IsSelectedEvent = is_selected_event,
                    //                    ServerIds = server_ids,
                    //                    IsSelectedServer = is_selected_server
                    //                });
                    //            }
                    //        }
                    //    }
                    //}
                }
                result.Add(result_disp);
                result.Add(result_sendto);
                result.Add(result_escalateto);
                result.Add(result_events);
                result.Add(result_servers);
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return result;
        }

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
                            SendTo = notificationDefinition.ScriptName
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
                    Response = Common.CreateResponse(result_sendto, "OK", "Hours and Destinations inserted successfully.");
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
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

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
                    Response = Common.CreateResponse(result_sendto, "OK", "Hours and Destinations inserted successfully.");
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
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

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
                    Response = Common.CreateResponse(result_scripts, "OK", "Hours and Destinations inserted successfully.");
                }
                else
                {
                    filterDef = scriptsRepository.Filter.Eq(x => x.Id, scriptDefinition.Id);
                    updateScripts = scriptsRepository.Updater.Set(x => x.ScriptName, scriptDefinition.ScriptName)
                       .Set(x => x.ScriptCommand, scriptDefinition.ScriptCommand)
                       .Set(x => x.ScriptLocation, scriptDefinition.ScriptLocation);
                    result = scriptsRepository.Update(filterDef, updateScripts);
                    Response = Common.CreateResponse(result);
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
                Console.WriteLine("error: " + ex.Message);
            }
            return Response;
        }

        [HttpPut("save_notification_definition")]
        public APIResponse UpdateNotificationDefinition([FromBody]NotificationsModel notificationDefinition)
        {
            //List<dynamic> result = new List<dynamic>();
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
                _id = notificationDefinition.ID;
                List<string> businessHoursIds = notificationDefinition.BusinessHoursIds;
                List<bool> selectedHours = notificationDefinition.IsSelectedHour;
                List<string> eventIds = notificationDefinition.EventIds;
                List<bool> selectedEvents = notificationDefinition.IsSelectedEvent;
                List<string> serverIds = notificationDefinition.ServerIds;
                List<bool> selectedServers = notificationDefinition.IsSelectedServer;

                notificationsRepository = new Repository<Notifications>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                eventsMasterRepository = new Repository<EventsMaster>(ConnectionString);

                notificationData = new List<string>();
                foreach (var hour in businessHoursIds)
                {
                    var ind = businessHoursIds.IndexOf(hour);
                    if (ind != -1)
                    {
                        var val = selectedHours[ind];
                        if (val)
                        {
                            notificationData.Add(hour);
                        }
                    }
                }

                serversData = new List<string>();
                foreach (var server in serverIds)
                {
                    var ind = serverIds.IndexOf(server);
                    if (ind != -1)
                    {
                        var val = selectedServers[ind];
                        if (val)
                        {
                            serversData.Add(server);
                        }
                    }
                }

                eventsData = new List<string>();
                foreach (var eventval in eventIds)
                {
                    var ind = eventIds.IndexOf(eventval);
                    if (ind != -1)
                    {
                        var val = selectedEvents[ind];
                        if (val)
                        {
                            eventsData.Add(eventval);
                        }
                    }
                }

                // Create a new notification definition
                if (_id == null || _id == "")
                {
                    // 1. Create a notifications document with a list of hours and destinations and escalation id's in SendList

                    Notifications notification = new Notifications();
                    notification.Id = ObjectId.GenerateNewId().ToString();
                    _id = notification.Id;
                    notification.NotificationName = notificationDefinition.NotificationName;
                    notification.SendList = notificationData;
                    notificationsRepository.Insert(notification);

                    // 2. Update the server documents with a list of notifications - add a notification to NotificationList

                    filterServerDef = serversRepository.Filter.In(x => x.Id, serversData);
                    var serverList = serversRepository.Find(filterServerDef);

                    foreach (var serverDef in serverList)
                    {
                        tempList = new List<string>();
                        if (serverDef.NotificationList != null)
                        {
                            var notificationList = serverDef.NotificationList.ToList();
                            int ind = notificationList.FindIndex(x => x == notification.Id);
                            if (ind < 0)
                            {
                                serverDef.NotificationList.Add(notification.Id);
                            }
                        }
                        else
                        {
                            tempList.Add(notification.Id);
                            serverDef.NotificationList = tempList;
                        }
                        serversRepository.Replace(serverDef);
                    }

                    // 3. Update the events_master documents with a list of notifications - add a notificaion to NotificationList

                    filterEventsDef = eventsMasterRepository.Filter.In(x => x.Id, eventsData);
                    var eventsList = eventsMasterRepository.Find(filterEventsDef);

                    foreach (var eventDef in eventsList)
                    {
                        tempList = new List<string>();
                        if (eventDef.NotificationList != null)
                        {
                            var notificationList = eventDef.NotificationList.ToList();
                            int ind = notificationList.FindIndex(x => x == notification.Id);
                            if (ind < 0)
                            {
                                eventDef.NotificationList.Add(notification.Id);
                            }                            
                        }
                        else
                        {
                            tempList.Add(notification.Id);
                            eventDef.NotificationList = tempList;                            
                        }
                        eventsMasterRepository.Replace(eventDef);
                    }
                    saveresult = GetNotificationsList();
                    Response = Common.CreateResponse(saveresult, "OK", "Hours and Destinations inserted successfully.");
                }
                // Update an existing notification definition
                else
                {
                    // 1. Update the notifications document with a list of hours and destinations and escalation id's

                    filterNotificationDef = notificationsRepository.Filter.Eq(x => x.Id, _id);
                    updateNotificationDef = notificationsRepository.Updater.Set(x => x.SendList, notificationData).Set(x => x.NotificationName, notificationDefinition.NotificationName);
                    var result = notificationsRepository.Update(filterNotificationDef, updateNotificationDef);

                    // 2. Update the server documents with a list of notifications. The algorithm is as follows:
                    //    2.1. Get server documents that have a NotificationList embedded document containing the notification id
                    //    2.2. Parse the documents and update the NotificationList of the ones that are no longer associated with the notification
                    //    2.3. Get server documents that match the id's listed in the notification - serversData
                    //    2.4. Update the NotificationList of the documents by adding the current notification id to NotificationList

                    filterServerDef = serversRepository.Filter.AnyEq(x => x.NotificationList, _id);
                    var serversList = serversRepository.Find(filterServerDef).ToList();

                    if (serversList.Count > 0)
                    {
                        foreach (var serverDef in serversList)
                        {
                            if (!serversData.Contains(serverDef.Id))
                            {
                                if (serverDef.NotificationList != null)
                                {
                                    var notificationList = serverDef.NotificationList.ToList();
                                    var itemToRemove = notificationList.Single(r => r == _id);
                                    serverDef.NotificationList.Remove(itemToRemove);
                                    serversRepository.Replace(serverDef);
                                }

                            }
                        }
                    }

                    filterServerDef = serversRepository.Filter.In(x => x.Id, serversData);
                    serversList = serversRepository.Find(filterServerDef).ToList();

                    if (serversList.Count > 0)
                    {
                        foreach (var serverDef in serversList)
                        {
                            tempList = new List<string>();
                            if (serverDef.NotificationList != null)
                            {
                                var notificationList = serverDef.NotificationList.ToList();
                                int ind = notificationList.FindIndex(x => x == _id);
                                if (ind < 0)
                                {
                                    serverDef.NotificationList.Add(_id);
                                } 
                            }
                            else
                            {
                                tempList.Add(_id);
                                serverDef.NotificationList = tempList;
                            }
                            serversRepository.Replace(serverDef);
                        }
                    }

                    // 3. Update the events_master documents with a list of notifications. The algorithm is as follows:
                    //    2.1. Get events_master documents that have a NotificationList embedded document containing the notification id
                    //    2.2. Parse the documents and update the NotificationList of the ones that are no longer associated with the notification
                    //    2.3. Get events_master documents that match the id's listed in the notification - eventsData
                    //    2.4. Update the NotificationList of the documents by adding the current notification id to NotificationList

                    filterEventsDef = eventsMasterRepository.Filter.AnyEq(x => x.NotificationList, _id);
                    var eventsList = eventsMasterRepository.Find(filterEventsDef).ToList();

                    if (eventsList.Count > 0)
                    {
                        foreach (var eventDef in eventsList)
                        {
                            if (!eventsData.Contains(eventDef.Id))
                            {
                                if (eventDef.NotificationList != null)
                                {
                                    var notificationList = eventDef.NotificationList.ToList();
                                    var itemToRemove = notificationList.Single(r => r == _id);
                                    eventDef.NotificationList.Remove(itemToRemove);
                                    eventsMasterRepository.Replace(eventDef);
                                }

                            }
                        }
                    }

                    filterEventsDef = eventsMasterRepository.Filter.In(x => x.Id, eventsData);
                    eventsList = eventsMasterRepository.Find(filterEventsDef).ToList();

                    if (eventsList.Count > 0)
                    {
                        foreach (var eventDef in eventsList)
                        {
                            tempList = new List<string>();
                            if (eventDef.NotificationList != null)
                            {
                                var notificationList = eventDef.NotificationList.ToList();
                                int ind = notificationList.FindIndex(x => x == _id);
                                if (ind < 0)
                                {
                                    eventDef.NotificationList.Add(_id);
                                }                                
                            }
                            else
                            {
                                tempList.Add(_id);
                                eventDef.NotificationList = tempList;
                            }
                            eventsMasterRepository.Replace(eventDef);
                        }
                    }
                    saveresult = GetNotificationsList();
                    Response = Common.CreateResponse(saveresult, "OK", "Hours and Destinations inserted successfully.");
                }
            }
            catch (Exception ex)
            {
                Response = Common.CreateResponse(null, "Error", ex.Message);
            }
            return Response;
        }

        [HttpDelete("delete_hours_destinations/{id}")]
        public void DeleteHoursDestinations(string id)
        {
            try
            {
                notificationDestRepository = new Repository<NotificationDestinations>(ConnectionString);
                Expression<Func<NotificationDestinations, bool>> expression = (p => p.Id == id);
                notificationDestRepository.Delete(expression);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Business Hours falied .\n Error Message :" + exception.Message);
            }
        }

        [HttpDelete("delete_script/{id}")]
        public void DeleteScript(string id)
        {
            try
            {
                scriptsRepository = new Repository<Scripts>(ConnectionString);
                Expression<Func<Scripts, bool>> expression = (p => p.Id == id);
                scriptsRepository.Delete(expression);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Business Hours falied .\n Error Message :" + exception.Message);
            }
        }

        [HttpDelete("delete_notification_definition/{id}")]
        public void DeleteNotificationDefinition(string id)
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
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Notification falied .\n Error Message :" + exception.Message);
            }
            

        }
        #endregion

        #endregion

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

        [HttpGet("get_device_type_list")]
        public APIResponse GetDeviceTypes()
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var result = serversRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.DeviceType, Value = x.DeviceType }).Distinct().ToList().OrderBy(x => x.DisplayText);


                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching Maintenance failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

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
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Fetching mobile users failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        [HttpGet("device_list")]
        public APIResponse GetAllServersWithLocation()
        {
            serversRepository = new Repository<Server>(ConnectionString);
            List<ServerLocation> serverLocations = new List<ServerLocation>();
            try
            {

                var result = serversRepository.Collection.Aggregate()
                                                         .Lookup("location", "location_id", "_id", "result").ToList();
                foreach (var x in result)
                {
                    ServerLocation serverLocation = new ServerLocation();
                    {
                        serverLocation.Id = x["_id"].AsObjectId.ToString();
                        serverLocation.DeviceName = x.GetValue("device_name", BsonString.Create(string.Empty)).ToString();
                        serverLocation.DeviceType = x.GetValue("device_type", BsonString.Create(string.Empty)).ToString();
                        serverLocation.Description = x.GetValue("description", BsonString.Create(string.Empty)).ToString();
                        serverLocation.AssignedNode = x.GetValue("assigned_node", BsonString.Create(string.Empty)).ToString();
                        serverLocation.CurrentNode = x.GetValue("current_node", BsonString.Create(string.Empty)).ToString();
                        if (x.GetValue("result", BsonValue.Create(string.Empty)).AsBsonArray.Values.Count() > 0)
                        {
                            serverLocation.LocationName = x.GetValue("result", BsonValue.Create(string.Empty))[0]["location_name"].ToString();
                        }
                        serverLocation.IsSelected = false;
                    }
                    serverLocations.Add(serverLocation);
                }
                Response = Common.CreateResponse(serverLocations.OrderBy(x => x.LocationName));
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

            }
            return Response;
        }


        [HttpPut("save_windows_services")]
        public APIResponse SaveWindowsServices([FromBody]DeviceSettings windowsservicesettings)
        {
            try
            {
                var windowsServiceValues = ((Newtonsoft.Json.Linq.JArray)windowsservicesettings.Value).ToObject<List<WindowsServicesValue>>();
                var devicesList = ((Newtonsoft.Json.Linq.JArray)windowsservicesettings.Devices).ToObject<string[]>();
                UpdateDefinition<Server> updateDefinition = null;
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
                Response = Common.CreateResponse(null, "Error", "Get maintain users falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }


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
                var updateDefination = serversRepository.Updater.Set(p => p.SimulationTests, nameValuePairs);
                var result = serversRepository.Update(server, updateDefination);
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Simulationtests updated successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save simulationtests falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(ibmsimulations);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", " simulationtest falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Node Health

        [HttpGet("get_nodes_health")]
        public APIResponse GetAllNodesHealth()
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                serversRepository = new Repository<Server>(ConnectionString);
                var result = nodesRepository.Collection.AsQueryable().Select(x => new NodesModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    HostName = x.HostName,
                    Pulse = x.Pulse,
                    IsAlive =x.IsAlive,
                    Alive =x.IsAlive?"Yes":"No",
                    LoadFactor = x.LoadFactor,
                    IsConfiguredPrimary = x.IsConfiguredPrimary,
                    IsPrimary = x.IsPrimary
                    


                }).ToList().OrderBy(x => x.Name);
                // var servicesresult =  nodesRepository.Collection.AsQueryable().Select(x => x.ServiceStatus).ToList();
               
                var nodesData = nodesRepository.All().Select(x => x.Name).Distinct().OrderBy(x => x).ToList();
                //  Response = Common.CreateResponse(result);

                // var serviceresult = nodesRepository.Collection.AsQueryable().Where(x => x.Id == servernodes.id).Select(x => x.ServiceStatus).FirstOrDefault();

              

                Response = Common.CreateResponse(new { nodesData = nodesData, result = result});

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "get Nodes Health falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("get_nodes_services")]
        public APIResponse GetAllNodesServices(string id)
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);
                Expression<Func<Nodes, bool>> expression = (p => p.Id == id);

                var serviceresult = nodesRepository.Find(expression).Select(x => x.ServiceStatus).FirstOrDefault();
                //   var distinctData = summaryStats.Select(x => new { DeviceId = x.DeviceID, DeviceName = x.DeviceName }).Distinct().OrderBy(x => x.DeviceName).ToList();
                List<ServiceStatusModel> service = new List<ServiceStatusModel>();
                List<NodesServices> nodesServicesStatus = new List<NodesServices>();
               
                    if (serviceresult != null)
                    {

                        NodesServices nodesservices = new NodesServices();


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

                        nodesServicesStatus.Add(nodesservices);
                    }
                    Response = Common.CreateResponse(nodesServicesStatus);

                
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "get Nodes Health falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_nodes_health")]
        public APIResponse UpdateNodesHealth([FromBody]NodesModel nodeshealth)
        {
            try
            {
                nodesRepository = new Repository<Nodes>(ConnectionString);


                FilterDefinition<Nodes> filterDefination = Builders<Nodes>.Filter.Where(p => p.Id == nodeshealth.Id);
                var updateDefination = nodesRepository.Updater.Set(p => p.Name, nodeshealth.Name)
                                                         .Set(p => p.HostName, nodeshealth.HostName)
                                                         .Set(p => p.Pulse, nodeshealth.Pulse)
                                                         .Set(p => p.IsAlive, nodeshealth.IsAlive)
                                                         .Set(p => p.LoadFactor, nodeshealth.LoadFactor)
                                                         .Set(p => p.IsConfiguredPrimary, nodeshealth.IsConfiguredPrimary)
                                                         .Set(p => p.IsPrimary, nodeshealth.IsPrimary);
                                                       

                var result = nodesRepository.Update(filterDefination, updateDefination);
                Response = Common.CreateResponse(result, "OK", "Nodes Health updated successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Nodes Health falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }


        [HttpPut("save_nodes_servers")]
        public APIResponse UpdateNodesServers([FromBody] DeviceSettings devicesettings)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                string selectedNode = Convert.ToString(devicesettings.Setting);
                var devicesList = ((Newtonsoft.Json.Linq.JArray)devicesettings.Devices).ToObject<List<string>>();
                foreach(var id in devicesList)
                {

                    FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == id);

                    var updateDefination = serversRepository.Updater.Set(p => p.AssignedNode, selectedNode);

                    var result = serversRepository.Update(filterDefination, updateDefination);
                }
            }


            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Log Scan Servers falied .\n Error Message :" + exception.Message);
            }
            return Response;

        }

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
                Response = Common.CreateResponse(null, "Error", "Delete Nodes Health falied .\n Error Message :" + exception.Message);
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
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get All mobile users failed.\n Error Message :" + exception.Message);
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
                var result = mobileDevicesRepository.Update(filterDefination, updateDefination);
                Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Mobile User deleted successfully");

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Mobile User delete failed.\n Error Message :" + exception.Message);
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
                var result = mobileDevicesRepository.All().Where(x => x.ThresholdSyncTime == null).Select(x => new MobileUserDevice
                {
                    UserName = x.UserName,
                    DeviceName = x.DeviceName,
                    DeviceId = x.DeviceID,
                    OperatingSystem = x.OSType,
                    Id = x.Id


                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get all mobile devices failed.\n Error Message :" + exception.Message);
            }
            return Response;
        }

        /// <summary>
        ///Save Mobile Users
        /// <author>Durga</author>
        /// </summary>
        /// <returns></returns>
        [HttpPut("save_mobileusers")]
        public APIResponse UpdateServerCredentials([FromBody]MobileUserDevice mobileUser)
        {
            try
            {
                mobileDevicesRepository = new Repository<MobileDevices>(ConnectionString);



                if (!string.IsNullOrEmpty(mobileUser.Id))
                {

                    FilterDefinition<MobileDevices> filterDefination = Builders<MobileDevices>.Filter.Where(p => p.Id == mobileUser.Id);
                    var updateDefination = mobileDevicesRepository.Updater.Set(p => p.ThresholdSyncTime, mobileUser.ThresholdSyncTime);
                    var result = mobileDevicesRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, Common.ResponseStatus.Success.ToDescription(), "Mobile users updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Save Mobile users falied .\n Error Message :" + exception.Message);
            }

            return Response;

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
            try
            {
                eventsdetectedRepository = new Repository<EventsDetected>(ConnectionString);
                var result = eventsdetectedRepository.All().Where(x => x.EventDismissed == null).Select(x => new AlertsModel
                {
                    DeviceType=x.DeviceType,
                    DeviceName = x.Device,
                    EventType = x.EventType,
                    Details = x.Details,
                    EventDetected=Convert.ToString(x.EventDetected)
                  
                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Get all open issues failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }
        #endregion

        #region Servers Import
        #region Domino
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
                    errorMessage = "All imported servers must be assigned to a location. There were no locations found. Please create at least one location entry using the 'Setup & Security - Maintain Server Locations' menu option.";
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
        [HttpGet("get_domino_import")]
        public APIResponse GetDominoImportData()
        {
            DominoServerImportModel model = new DominoServerImportModel();
            model.DominoServer = Common.GetNameValue("Primary Server").Value;
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
            return Common.CreateResponse(model);
        }


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
                Response = Common.CreateResponse("Success");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }
        #endregion


        #region WebSphere

        [HttpPut("get_websohere_nodes")]
        public APIResponse LoadWebSphereNodes([FromBody]CellInfo cellInfo)
        {

            try
            {
                byte[] password;
                string decryptedPassword = string.Empty;
                string errorMessage = string.Empty;


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

                            var result = getServerList(cellInfo);
                        }
                        else
                        {
                            errorMessage = "Notes password may not be empty. Please update the password under Stored Passwords & Options\\IBM Domino Settings.";
                            throw new Exception(errorMessage);
                        }

                    }

                }
                catch (Exception exception)
                {
                    Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
                }

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }


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

        private object DecodeXMLFromPath(string pathToXML, Type type)
        {
            XmlSerializer serializer = new XmlSerializer(type);

            XmlDocument doc = new XmlDocument();
            doc.Load(pathToXML);

            XmlNodeReader reader = new XmlNodeReader(doc);

            object obj = serializer.Deserialize(reader);

            return obj;
        }

        [HttpGet("get_websohere_import")]
        public APIResponse GetWebSphereImportData()
        {
            WebShpereServerImport model = new WebShpereServerImport();
            serversRepository = new Repository<Server>(ConnectionString);
            model.CellsData = serversRepository.Collection.AsQueryable().Where(x => x.DeviceType == "WebSphere" && x.CellName != null)
                                                                        .Select(x => new CellInfo
                                                                        {
                                                                            DeviceId = x.Id,

                                                                            CellId = x.CellId,
                                                                            CellName = x.CellName,
                                                                            Name = x.DeviceName,
                                                                            HostName = x.CellHostName,
                                                                            PortNo = x.PortNumber,
                                                                            ConnectionType = x.ConnectionType,
                                                                            GlobalSecurity = x.GlobalSecurity,
                                                                            CredentialsId = x.CredentialsId,
                                                                            Realm = x.Realm
                                                                        }).ToList();
            deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
            model.DeviceAttributes = deviceAttributesRepository.All().Where(x => (x.DeviceType == "WebSphere")).Select(x => new DeviceAttributesModel
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

            model.CredentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText).ToList();
            foreach (var item in model.CellsData)
            {
                var credential = model.CredentialsData.FirstOrDefault(x => x.Value == item.CredentialsId);
                if(credential!=null)
                item.CredentialsName = credential.DisplayText;
            }
            return Common.CreateResponse(model);
        }


        [HttpPut("save_websphere_servers")]
        public APIResponse SaveWebSphereServers([FromBody]DominoServerImportModel serverImport)
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
                Response = Common.CreateResponse("Success");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;
        }

        [HttpPut("save_websphere_cell")]
        public APIResponse SaveWebsphereCellInfo([FromBody]CellInfo cellInfo)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {  if (string.IsNullOrEmpty(cellInfo.DeviceId))
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
                    server.DeviceType = "WebSphere";                   
                    serversRepository.Insert(server);
                }
            else
                {

                    FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id == cellInfo.DeviceId);
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
                    Response = Common.CreateResponse(result, "OK", "Cell information updated successfully");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);
            }
            return Response;

        }
        [HttpDelete("delete_cellInfo/{id}")]
        public void DeleteCellInfo(string id)
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> expression = (p => p.Id == id);
                serversRepository.Delete(expression);

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Business Hours falied .\n Error Message :" + exception.Message);
            }



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
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                Expression<Func<BusinessHours, bool>> businessHoursExpression = (p => p.DeviceId == Id);
                businessHoursRepository.Delete(businessHoursExpression);

                serverOtherRepository = new Repository<ServerOther>(ConnectionString);
                Expression<Func<ServerOther, bool>> serverOtherExpression = (p => p.Id == Id);
                businessHoursRepository.Delete(businessHoursExpression);

                serversRepository = new Repository<Server>(ConnectionString);
                Expression<Func<Server, bool>> serverExpression = (p => p.Id == Id);
                serversRepository.Delete(serverExpression);

                Response = Common.CreateResponse(null, Common.ResponseStatus.Success.ToDescription(), "Server Deleted sucessfully.");
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server Delete falied .\n Error Message :" + exception.Message);
            }

           return Response;
        }
        #endregion 

        #region Log Settings
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


                Response = Common.CreateResponse(new { loglevel =loglevel,logfilenames = logfilenames, combolist = combolist });
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

      

        [HttpGet("get_read_files/{logfile}")]
        public APIResponse GetreagLogFiles(string logfile)
        {
            try
            {
                var Text = "";
                nameValueRepository = new Repository<NameValue>(ConnectionString);

                var logfiles = nameValueRepository.Collection.AsQueryable().Where(x => x.Name == "Log Files Path-New").Select(x => x.Value).FirstOrDefault();
                string filepath = logfiles+"\\" + logfile;
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
                Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }


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
                var updateDefination = nameValueRepository.Updater.Set(p => p.Name, loglevel);
                var filterDefination = Builders<NameValue>.Filter.Where(p => p.Name == "Log Level");
                var update = nameValueRepository.Update(filterDefination, updateDefination);
                Response = Common.CreateResponse(update);
                //  var logslist = ((Newtonsoft.Json.Linq.JObject)devicesettings.FileName).ToObject<List<LogFolders>>();
                List<string> listoffiles =new  List<string>();
               
                
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
                    }
                }
 
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Log Scan Servers falied .\n Error Message :" + exception.Message);
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
                       DeviceName = serverData.DeviceName   ,
                        DeviceType = serverData.DeviceType,
                        Description = serverData.Description,
                        LocationId = serverData.LocationId,
                        IPAddress = serverData.IpAddress,
                        BusinessHoursId = serverData.BusinessHoursId,
                        MonthlyOperatingCost = serverData.MonthlyOperatingCost,
                        IdealUserCount = serverData.IdealUserCount,
                        Category=serverData.Category
                    };


                    string id = serversRepository.Insert(servers);
                    Response = Common.CreateResponse(id, Common.ResponseStatus.Success.ToDescription(), "Server inserted successfully.");
                


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, Common.ResponseStatus.Error.ToDescription(), "Server insert falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }
        #endregion

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
                    if (fileName.ToLower().Contains (".csv"))
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

    }
}

    

