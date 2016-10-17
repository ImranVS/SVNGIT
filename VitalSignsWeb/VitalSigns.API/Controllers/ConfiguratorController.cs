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



namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class ConfiguratorController : BaseController
    {

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

        private IRepository<NameValue> namevalueRepository;





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
                    countryData.Insert(0, "-All-");
                }
                if (!string.IsNullOrEmpty(country))
                {
                    var stateData = validLocationsRepository.All().FirstOrDefault(x => x.Country == country).States;
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
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
        }

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
                    Response = Common.CreateResponse(true, "OK", "Location inserted successfully");
                }
                else
                {
                    FilterDefinition<Location> filterDefination = Builders<Location>.Filter.Where(p => p.Id == locations.Id);
                    var updateDefination = locationRepository.Updater.Set(p => p.LocationName, locations.LocationName)
                                                             .Set(p => p.City, locations.City)
                                                             .Set(p => p.Country, locations.Country)
                                                             .Set(p => p.Region, locations.Region);
                    var result = locationRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "Location updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save locations falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_location/{id}")]
        public void DeleteLocation(string id)
        {
            locationRepository = new Repository<Location>(ConnectionString);
            Expression<Func<Location, bool>> expression = (p => p.Id == id);
            locationRepository.Delete(expression);
        }

        [HttpGet("business_hours")]
        public APIResponse GetAllBusinessHours()
        {
            try
            {


                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
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
                    Saturday = x.Days.Contains("Saturday")
                }).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching business hours failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

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

                if (string.IsNullOrEmpty(businesshour.Id))
                {
                    BusinessHours businessHours = new BusinessHours { Name = businesshour.Name, StartTime = businesshour.StartTime, Duration = businesshour.Duration, Days = days.ToArray() };
                    string id = businessHoursRepository.Insert(businessHours);
                    Response = Common.CreateResponse(id, "OK", "Business hour inserted successfully");
                }
                else
                {
                    FilterDefinition<BusinessHours> filterDefination = Builders<BusinessHours>.Filter.Where(p => p.Id == businesshour.Id);
                    var updateDefination = businessHoursRepository.Updater.Set(p => p.Name, businesshour.Name)
                                                             .Set(p => p.Duration, businesshour.Duration)
                                                             .Set(p => p.StartTime, businesshour.StartTime)
                                                             .Set(p => p.Days, days.ToArray());
                    var result = businessHoursRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "Business hour updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save business hours falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }

        [HttpDelete("delete_business_hours/{id}")]
        public void DeleteBusinessHours(string id)
        {
            try
            {
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                Expression<Func<BusinessHours, bool>> expression = (p => p.Id == id);
                businessHoursRepository.Delete(expression);

            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Business Hours falied .\n Error Message :" + exception.Message);
            }



        }
        //[HttpGet("credentials/{UserId}")]
        //public Credentials GetCredentials(string UserId)
        //{
        //    credentialsRepository = new Repository<Credentials>(ConnectionString);
        //    Expression<Func<Credentials, bool>> expression = (p => p.UserId == UserId);
        //    var result = credentialsRepository.Find(expression).FirstOrDefault();


        //    return result;
        //}

        [HttpPut("save_server_credentials")]
        public APIResponse UpdateServerCredentials([FromBody]ServerCredentialsModel serverCredential)
        {
            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);



                if (string.IsNullOrEmpty(serverCredential.Id))
                {
                    Credentials serverCredentials = new Credentials { Alias = serverCredential.Alias, Password = serverCredential.Password, DeviceType = serverCredential.DeviceType, UserId = serverCredential.UserId };


                    string id = credentialsRepository.Insert(serverCredentials);
                    Response = Common.CreateResponse(id, "OK", "Server Credential inserted successfully");
                }
                else
                {
                    FilterDefinition<Credentials> filterDefination = Builders<Credentials>.Filter.Where(p => p.Id == serverCredential.Id);
                    var updateDefination = credentialsRepository.Updater.Set(p => p.Alias, serverCredential.Alias)
                                                             .Set(p => p.DeviceType, serverCredential.DeviceType)
                                                             .Set(p => p.Password, serverCredential.Password)
                                                             .Set(p => p.UserId, serverCredential.UserId);
                    var result = credentialsRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "Server Credential updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Server Credentials falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }

        [HttpDelete("delete_credential/{Id}")]
        public void DeleteCredential(string Id)
        {
            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                Expression<Func<Credentials, bool>> expression = (p => p.Id == Id);
                credentialsRepository.Delete(expression);


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
            }
        }

        [HttpGet("credentials")]
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
                    Password = x.Password,
                    Id = x.Id


                }).ToList();


                Response = Common.CreateResponse(result);

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Server Credentials falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("ibm_domino_settings")]
        public APIResponse GetIbmDominoSettings()
        {
            nameValueRepository = new Repository<NameValue>(ConnectionString);
            var result = nameValueRepository.All().Select(x => new NameValue
            {
                Name = x.Name,
                Value = x.Value,
                Id = x.Id

            }).ToList();


            Response = Common.CreateResponse(result);
            return Response;


        }

        [HttpGet("maintenance")]
        public APIResponse GetAllMaintenance()
        {
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                var result = maintenanceRepository.All().Select(x => new MaintenanceModel
                {
                    Id = x.Id,
                    Name = x.Name,
                    StartDate = x.StartDate,
                    StartTime = x.StartTime,
                    EndDate = x.EndDate,
                    Duration = x.Duration,
                    MaintenanceFrequency = x.MaintenanceFrequency,
                    MaintenanceDaysList = x.MaintenanceDaysList,
                    ContinueForever = x.ContinueForever

                }).ToList();
                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching Maintenance failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        [HttpGet("servers_list")]
        public APIResponse GetAllServersList()
        {
            try
            {
                serversRepository = new Repository<Server>(ConnectionString);
                var result = serversRepository.All().Select(x => new ServersModel
                {

                    ServerName = x.DeviceName,
                    ServerType = x.DeviceType,
                    Description = x.Description,
                    ServerId = x.ServerId


                }).ToList();


                Response = Common.CreateResponse(result);
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Fetching Maintenance failed .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpGet("get_Device_type__list")]
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

        //[HttpGet("mobileusers")]
        //public APIResponse GetAllMobileUsersList()
        //{
        //    try
        //    {
        //        mobiledevicesRepository = new Repository<MobileDevices>(ConnectionString);
        //        var result = mobiledevicesRepository.All().Select(x => new MobileUserDevice
        //        {
        //            UserName = x.UserName,
        //            DeviceName = x.DeviceName,
        //           LastSyncTime = x.LastSyncTime



        //        }).ToList();


        //        Response = Common.CreateResponse(result);
        //    }
        //    catch (Exception exception)
        //    {
        //        Response = Common.CreateResponse(null, "Error", "Fetching Maintenance failed .\n Error Message :" + exception.Message);
        //    }
        //    return Response;
        //}


        [HttpGet("maintain_users")]
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
                Response = Common.CreateResponse(null, "Error", "Get maintain users falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }

        [HttpPut("save_maintain_users")]
        public APIResponse UpdateMaintainUsers([FromBody]MaintainUsersModel maintainuser)
        {
            try
            {
                maintainUsersRepository = new Repository<Users>(ConnectionString);


                if (string.IsNullOrEmpty(maintainuser.Id))
                {
                    Users maintainUsers = new Users { LoginName = maintainuser.LoginName, FullName = maintainuser.FullName, Email = maintainuser.Email, Status = maintainuser.Status, SuperAdmin = maintainuser.SuperAdmin, ConfiguratorAccess = maintainuser.ConfiguratorAccess, ConsoleCommandAccess = maintainuser.ConsoleCommandAccess };
                    maintainUsersRepository.Insert(maintainUsers);
                    Response = Common.CreateResponse(true, "OK", "Maintain Users inserted successfully");
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
                    Response = Common.CreateResponse(result, "OK", "Maintain Users updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Maintain Users falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }

        [HttpDelete("delete_maintain_users/{id}")]
        public void DeleteMaintainUsers(string id)
        {
            maintainUsersRepository = new Repository<Users>(ConnectionString);
            Expression<Func<Users, bool>> expression = (p => p.Id == id);
            maintainUsersRepository.Delete(expression);
        }

        [HttpPut("save_maintenancedata")]
        public APIResponse UpdateMaintenancedata([FromBody]MaintenanceModel maintenance)
        {
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);



                if (string.IsNullOrEmpty(maintenance.Id))
                {
                    Maintenance maintenancedata = new Maintenance { Name = maintenance.Name, StartDate = maintenance.StartDate, StartTime = maintenance.StartTime, Duration = maintenance.Duration, EndDate = maintenance.EndDate, MaintenanceDaysList = maintenance.MaintenanceDaysList };


                    string id = maintenanceRepository.Insert(maintenancedata);
                    Response = Common.CreateResponse(id, "OK", "Maintenancedata inserted successfully");
                }
                else
                {
                    FilterDefinition<Maintenance> filterDefination = Builders<Maintenance>.Filter.Where(p => p.Id == maintenance.Id);
                    var updateDefination = maintenanceRepository.Updater.Set(p => p.Name, maintenance.Name)
                                                             .Set(p => p.StartDate, maintenance.StartDate)
                                                             .Set(p => p.StartTime, maintenance.StartTime)
                                                             .Set(p => p.Duration, maintenance.Duration)
                                                             .Set(p => p.EndDate, maintenance.EndDate)
                                                             .Set(p => p.MaintenanceDaysList, maintenance.MaintenanceDaysList);
                    var result = maintenanceRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "Maintenancedata  updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Maintenancedata falied .\n Error Message :" + exception.Message);
            }

            return Response;

        }

        [HttpDelete("delete_maintenancedata/{id}")]
        public void DeleteMaintenancedata(string id)
        {
            try
            {
                maintenanceRepository = new Repository<Maintenance>(ConnectionString);
                Expression<Func<Maintenance, bool>> expression = (p => p.Id == id);
                maintenanceRepository.Delete(expression);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Delete Maintenancedata falied .\n Error Message :" + exception.Message);
            }
        }

        [HttpGet("travelerdatastore")]
        public APIResponse GetAllTravelerDataStore()
        {
            try
            {
                travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
                var result = travelerdatastoreRepository.All().Select(x => new TravelerDataStoresModel
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
                    TestScanServer = x.TestScanServer,
                    UsedByServers = x.UsedByServers

                }).ToList();

                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Get traveler data falied .\n Error Message :" + exception.Message);
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
                    TravelerDTS travelerds = new TravelerDTS { TravelerServicePoolName = travelerdatas.TravelerServicePoolName, DeviceName = travelerdatas.DeviceName, DataStore = travelerdatas.DataStore, DatabaseName = travelerdatas.DatabaseName, Port = travelerdatas.Port, UserName = travelerdatas.UserName, Password = travelerdatas.Password, IntegratedSecurity = travelerdatas.IntegratedSecurity, TestScanServer = travelerdatas.TestScanServer, UsedByServers = travelerdatas.UsedByServers };
                    travelerdatastoreRepository.Insert(travelerds);
                    Response = Common.CreateResponse(true, "OK", "traveler data inserted successfully");
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
                                                             .Set(p => p.TestScanServer, travelerdatas.TestScanServer)
                                                             .Set(p => p.UsedByServers, travelerdatas.UsedByServers);
                    var result = travelerdatastoreRepository.Update(filterDefination, updateDefination);
                    Response = Common.CreateResponse(result, "OK", "traveler data updated successfully");
                }
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save traveler data falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }


        [HttpDelete("delete_traveler_data_store/{id}")]
        public void DeleteTravelerDataStore(string id)
        {
            travelerdatastoreRepository = new Repository<TravelerDTS>(ConnectionString);
            Expression<Func<TravelerDTS, bool>> expression = (p => p.Id == id);
            travelerdatastoreRepository.Delete(expression);
        }


        [HttpGet("get_server_credentials_businesshours")]
        public APIResponse GetDeviceListDropDownData()
        {

            try
            {
                credentialsRepository = new Repository<Credentials>(ConnectionString);
                businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
                locationRepository = new Repository<Location>(ConnectionString);
                var credentialsData = credentialsRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Alias, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var businessHoursData = businessHoursRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.Name, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                var locationsData = locationRepository.Collection.AsQueryable().Select(x => new ComboBoxListItem { DisplayText = x.LocationName, Value = x.Id }).ToList().OrderBy(x => x.DisplayText);
                Response = Common.CreateResponse(new { credentialsData = credentialsData, businessHoursData = businessHoursData, locationsData = locationsData });
                return Response;
            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", exception.Message);

                return Response;
            }
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
                        if (!string.IsNullOrEmpty(x.GetValue("result", BsonValue.Create(string.Empty)).ToString()))
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

        [HttpGet("get_device_attributes")]
        public APIResponse GetDeviceAttributes()
        {
            try
            {
                deviceAttributesRepository = new Repository<DeviceAttributes>(ConnectionString);
                var result = deviceAttributesRepository.All().Select(x => new DeviceAttributesModel
                {
                    Id = x.Id,
                    AttributeName = x.AttributeName,
                    DefaultValue = x.DefaultValue,
                    DeviceType = x.DeviceType,
                    FieldName = x.FieldName,
                    Unitofmeasurement = x.Unitofmeasurement,
                    IsSelected = false
                }).ToList();
                Response = Common.CreateResponse(result);
            }

            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Get maintain users falied .\n Error Message :" + exception.Message);
            }
            return Response;
        }


        [HttpPut("save_server_credentials_businesshours")]
        public APIResponse UpdateIbmDominoSettings([FromBody]DeviceSettings deviceSettings)
        {
            serversRepository = new Repository<Server>(ConnectionString);
            try
            {
                string setting = Convert.ToString(deviceSettings.Setting);
                string settingValue = Convert.ToString(deviceSettings.Value);
                string devices = Convert.ToString(deviceSettings.Devices);
                UpdateDefinition<Server> updateDefinition = null;

                if (!string.IsNullOrEmpty(devices))
                {
                    //var devicesList = devices.Replace('[',' ').Replace(']', ' ').Trim().Split(',');
                    var devicesList = ((Newtonsoft.Json.Linq.JArray)deviceSettings.Devices).ToObject<string[]>();
                    foreach(string id in devicesList)
                    {

                        FilterDefinition<Server> filterDefination = Builders<Server>.Filter.Where(p => p.Id==id);
                        if (setting.Equals("locations"))
                            updateDefinition = serversRepository.Updater.Set(p => p.LocationId, settingValue);
                        else if (setting.Equals("credentials"))
                            updateDefinition = serversRepository.Updater.Set(p => p.CredentialsId, settingValue);
                        else if (setting.Equals("businessHours"))

                            updateDefinition = serversRepository.Updater.Set(p => p.BusinessHoursId, settingValue);
                        if (updateDefinition != null)
                        {
                            var result = serversRepository.Update(filterDefination, updateDefinition);
                            Response = Common.CreateResponse(result, "OK", "Location updated successfully");
                        }
                        else
                        {
                            Response = Common.CreateResponse(null, "OK", "Settings are not selected");
                        }
                    }
                    
                }
                else
                {
                    Response = Common.CreateResponse(null, "Error", "Devices were not selected");
                }


            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save Server Credentials falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }


        [HttpPut("save_preferences")]
        public APIResponse UpdatePreferences([FromBody]PreferencesModel userpreference)
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

                namevalueRepository = new Repository<NameValue>(ConnectionString);

                foreach (var setting in preferencesSettings)
                {
                    if (namevalueRepository.Collection.AsQueryable().Where(x => x.Name.Equals(setting.Name)).Count() > 0)
                    {
                        var filterDefination = Builders<NameValue>.Filter.Where(p => p.Name == setting.Name);
                        var updateDefinitaion = namevalueRepository.Updater.Set(p => p.Value, setting.Value);
                        var results = namevalueRepository.Update(filterDefination, updateDefinitaion);
                    }
                    else
                    {
                        namevalueRepository.Insert(setting);
                    }
                }

            }
            catch (Exception exception)
            {
                Response = Common.CreateResponse(null, "Error", "Save preferences falied .\n Error Message :" + exception.Message);
            }

            return Response;
        }


    }
}
