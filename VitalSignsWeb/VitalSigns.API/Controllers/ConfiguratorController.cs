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
                    businessHoursRepository.Insert(businessHours);
                    Response = Common.CreateResponse(true, "OK", "Business hour inserted successfully");
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



        [HttpDelete("{id}/delete_business_hours")]
        public void DeleteBusinessHours(string id)
        {
            businessHoursRepository = new Repository<BusinessHours>(ConnectionString);
            Expression<Func<BusinessHours, bool>> expression = (p => p.Id == id);
            businessHoursRepository.Delete(expression);



        }
    }
}
