using Microsoft.AspNet.Mvc;
using VitalSigns.API.Models;


namespace VitalSigns.API.Controllers
{
    public class BaseController : Controller
    {
        //public BaseController()
        //{
        //    Response = new APIResponse();
        //}
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
        protected APIResponse Response { get; set;}
    }
}
