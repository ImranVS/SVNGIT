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


namespace VitalSigns.API.Controllers
{
    [Route("[controller]")]
    public class ConfiguratorController :BaseController
    {

        private IRepository<BusinessHours> businessHoursRepository;

        [HttpGet("business_hours")]
        public APIResponse GetAllBusinessHours()
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
            var result = businessHoursRepository.All().Select(x => new BusinessHoursModel
            {
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
            return Response;
        }
        [HttpGet("{device_id}/business_hours")]
        public BusinessHours GetBusinessHours( string device_id)
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
            Expression<Func<BusinessHours, bool>> expression = (p => p.DeviceId == device_id);
            var result = businessHoursRepository.Find(expression).FirstOrDefault();


            return result;
        }
        [HttpPut("update_business_hours")]
        public bool UpdateBusinessHours([FromBody]BusinessHours businesshours)
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
            //Expression<Func<Location, bool>> expression = (p => p.Id == location.Id);
            FilterDefinition<BusinessHours> filterDefination = Builders<BusinessHours>.Filter.Where(p => p.Id == businesshours.Id);
            var updateDefination = businessHoursRepository.Updater.Set(p => p.Name, businesshours.Name)
                                                     .Set(p => p.Duration, businesshours.Duration)
                                                     .Set(p => p.StartTime, businesshours.StartTime)
                                                     .Set(p => p.Days, businesshours.Days);
            var result = businessHoursRepository.Update(filterDefination, updateDefination);


            return result;
        }

        [HttpPut("insert_business_hours")]
        public void InsertBusinessHours([FromBody]BusinessHours businesshours)
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);

            businessHoursRepository.Insert(businesshours);

        }

        [HttpGet("{device_id}/delete_business_hours")]
        public void DeleteLocation(string device_id)
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
            Expression<Func<BusinessHours, bool>> expression = (p => p.DeviceId == device_id);
            businessHoursRepository.Delete(expression);



        }
    }
}
