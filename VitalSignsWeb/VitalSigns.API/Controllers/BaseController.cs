using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;

// For more information on enabling MVC for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace VitalSigns.API.Controllers
{
    public class BaseController : Controller
    {
        public string ConnectionString
        {
            get { return Startup.ConnectionString + @"/" + Startup.DataBaseName; }
        }
        private string _tenantId;
        public string TenantId
        {
            get {
                if (string.IsNullOrEmpty(_tenantId))
                {
                    _tenantId = "";
                }

                return _tenantId; }
         
        }
    }
}
