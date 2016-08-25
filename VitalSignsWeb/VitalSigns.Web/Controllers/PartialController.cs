using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;
using MongoDB.Bson;
using VitalSigns.Web.Models;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace VitalSigns.Web.Controllers
{

    [Route("[controller]/[action]")]
    public class PartialController : Controller
    {

        public IActionResult SiteMap()
        {
            var dataContext = new DataContext();

            var menu = dataContext.SiteMap.Find(x => x.Id == "left").SingleOrDefault();
            
            return PartialView(menu);
        }

        public IActionResult Navigator()
        {
            var dataContext = new DataContext();

            var menu = dataContext.SiteMap.Find(x => x.Id == "navigator").SingleOrDefault();

            return PartialView(menu);
        }

    }
}
