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

            return dataContext.Profiles.Find(new BsonDocument("_id", email)).First();
        }

        #region more routing samples
        //[HttpGet("byname/{name}")]
        //public Profile GetProfileFromName(string name)
        //{
        //    var dataContext = new DataContext();

        //    return dataContext.Profiles.Find(new BsonDocument("_id", name)).First();
        //}

        //[HttpGet("{id}")]
        //public Profile GetProfileFromID(int id)
        //{
        //    var dataContext = new DataContext();

        //    return dataContext.Profiles.Find(new BsonDocument("_id", id)).First();
        //}
        #endregion

        [HttpDelete("{email}")]
        public object DeleteProfileFromEmail(string email)
        {
            var dataContext = new DataContext();

            dataContext.Profiles.DeleteOne(new BsonDocument("_id", email));

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
                new BsonDocument("_id", email),
                profile,
                new FindOneAndReplaceOptions<Profile>
                {
                    IsUpsert = true
                });
        }
        
    }
}
