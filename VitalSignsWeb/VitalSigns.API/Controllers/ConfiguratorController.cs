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


namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class ConfiguratorController :BaseController
    {

        private IRepository<BusinessHours> businessHoursRepository;
        private IRepository<Credentials> credentialsRepository;
        private IRepository<Location> locationRepository;

        private IRepository<ValidLocation> validLocationsRepository;

        private IRepository<Maintenance> maintenanceRepository;

        private IRepository<NameValue> nameValueRepository;


        private IRepository<Server> serversRepository;

        private IRepository<MobileDevices> mobiledevicesRepository;





        [HttpGet("get_locations")]
        public APIResponse GetLocationsDropDownData(string country,string state)
        {

            try
            {
                validLocationsRepository = new Repository<ValidLocation>(ConnectionString);
                var countryData = validLocationsRepository.All().Where(x => x.Country != null).Select(x => x.Country).Distinct().OrderBy(x => x).ToList();
                Response = Common.CreateResponse(new { countryData = countryData });
                if (!string.IsNullOrEmpty(country))
                {
                    var stateData = validLocationsRepository.All().Where(x => x.States != null && x.Country == country).Select(x => x.States).Distinct().OrderBy(x => x).ToList();
                    stateData.Insert(0, stateData.FirstOrDefault());
                    Response = Common.CreateResponse(new { countryData = countryData,stateData=stateData });
                }
               

                countryData.Insert(0, "-All-");
                


                
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

        [HttpDelete("{id}/delete_location")]
        public void DeleteLocation(string id)
        {
            locationRepository = new Repository<Location>(ConnectionString);
            Expression<Func<Location, bool>> expression = (p => p.Id == id);
            locationRepository.Delete(expression);
        }

        private IRepository<MaintainUser> maintainUsersRepository;


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
            catch(Exception exception)
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
                    string id=  businessHoursRepository.Insert(businessHours);
                    Response = Common.CreateResponse( id, "OK", "Business hour inserted successfully");
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
            }catch(Exception exception)
            {
                Response = Common.CreateResponse(null, "Error","Save business hours falied .\n Error Message :"+ exception.Message);
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

            catch(Exception exception)
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
                    Credentials serverCredentials = new Credentials { Alias = serverCredential.Alias, Password = serverCredential.Password, DeviceType = serverCredential.DeviceType,UserId=serverCredential.UserId};

                    
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
               Name=x.Name,
               Value=x.Value,
               Id = x.Id
               
            }).ToList();


            Response = Common.CreateResponse(result);
            return Response;


        }

        [HttpGet("maintenance")]
        public APIResponse GetAllMaintenance()
        {
            try { 
            maintenanceRepository = new Repository<Maintenance>(ConnectionString);
            var result = maintenanceRepository.All().Select(x => new MaintenanceModel
            {
                Id = x.Id,
                Name = x.Name,
                StartDate =  x.StartDate,
                StartTime = x.StartTime,
                EndDate = x.EndDate,
                Duration = x.Duration,
                MaintenanceFrequency = x.MaintenanceFrequency,
                MaintenanceDaysList = x.MaintenanceDaysList,
                ContinueForever = x.ContinueForever

            }).ToList();
            Response = Common.CreateResponse(result);
            }
            catch(Exception exception)
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
                maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);
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
                maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);


                if (string.IsNullOrEmpty(maintainuser.Id))
                {
                    MaintainUser maintainUsers = new MaintainUser { LoginName = maintainuser.LoginName, FullName = maintainuser.FullName, Email = maintainuser.Email, Status = maintainuser.Status, SuperAdmin = maintainuser.SuperAdmin, ConfiguratorAccess = maintainuser.ConfiguratorAccess, ConsoleCommandAccess = maintainuser.ConsoleCommandAccess };
                    maintainUsersRepository.Insert(maintainUsers);
                    Response = Common.CreateResponse(true, "OK", "Maintain Users inserted successfully");
                }
                else
                {
                    FilterDefinition<MaintainUser> filterDefination = Builders<MaintainUser>.Filter.Where(p => p.Id == maintainuser.Id);
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
            maintainUsersRepository = new Repository<MaintainUser>(ConnectionString);
            Expression<Func<MaintainUser, bool>> expression = (p => p.Id == id);
            maintainUsersRepository.Delete(expression);
        }

    }
}
