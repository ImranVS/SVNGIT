using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using VitalSigns.API.Models.Charts;
using MongoDB.Bson.Serialization;
using Microsoft.AspNet.Authorization;

namespace VitalSigns.API.Controllers
{

    [Route("[controller]")]
    public class ProfilesController : Controller
    {

        [HttpGet()]
        public List<Profile> GetAllProfiles()
        {
            var dataContext = new DataContext();

            return dataContext.Profiles.Find(new BsonDocument()).ToList();
        }

        [HttpGet("{email}")]
        public Profile GetProfileFromEmail(string email)
        {
            var dataContext = new DataContext();
            
            // TODO: retrieve tenant ID from configuration
            return dataContext.Profiles.Find(p => p.TenantId == 5 && p.Email == email).First();
        }
        
        [HttpDelete("{email}")]
        public object DeleteProfileFromEmail(string email)
        {
            var dataContext = new DataContext();

            dataContext.Profiles.DeleteOne(new BsonDocument("email", email));

            return new
            {
                email = email,
                operation = "delete",
                status = "OK"
            };
        }

        // TODO: Decide between /profiles or /profiles/email (routing)
        // TODO: Consider sending back an object with operation status (create or update) along with the profile
        [HttpPut("{email}")]
        public Profile ReplaceProfileFromEmail(string email, [FromBody]Profile profile)
        {
            var dataContext = new DataContext();

            return dataContext.Profiles.FindOneAndReplace(
                new BsonDocument("email", email),
                profile,
                new FindOneAndReplaceOptions<Profile>
                {
                    IsUpsert = true
                });
        }
        
    }
}
