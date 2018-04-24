using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class WindowsServiceModel
    {
        [JsonProperty("server_required")]
        public bool ServerRequired { get; set; }

        [JsonProperty("service_name")]
        public string ServiceName { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        [JsonProperty("startup_mode")]
        public string StartupMode { get; set; }

        [JsonProperty("monitored")]
        public bool Monitored { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }
    }

    public class WindowsServicesValue
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("service_name")]
        public string ServiceName { get; set; }

       

    }
}
