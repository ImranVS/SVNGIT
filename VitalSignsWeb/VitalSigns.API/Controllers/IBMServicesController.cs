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

    [Route("services")]
    public class IBMServicesController : Controller
    {


        [HttpGet("foo-service")]
        public object Foo()
        {

            // TODO : implement this method on the same concept as VitalSigns.API.Controllers.ServicesController
            return "foo";
        }
        
    }
}
