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
    public class NavigationController : BaseController
    {

        [HttpGet("sitemaps")]
        public List<SiteMap> GetSiteMaps()
        {

            var dataContext = new DataContext();

            return dataContext.SiteMaps.Find(new BsonDocument()).ToList();

        }

        [HttpGet("sitemaps/{siteMap}")]
        public SiteMap GetSiteMap(string siteMap)
        {

            var dataContext = new DataContext();

            return dataContext.SiteMaps.Find(new BsonDocument("_id", siteMap)).First();

        }

        [HttpPut("sitemaps")]
        public SiteMap ReplaceProfileFromEmail([FromBody]SiteMap siteMap)
        {

            var dataContext = new DataContext();

            return dataContext.SiteMaps.FindOneAndReplace(
                new BsonDocument("_id", siteMap.Id),
                siteMap,
                new FindOneAndReplaceOptions<SiteMap>
                {
                    IsUpsert = true
                });

        }

    }
}
