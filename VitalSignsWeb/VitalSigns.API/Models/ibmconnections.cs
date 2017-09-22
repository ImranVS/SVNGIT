using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ibmconnections
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("name")]
        public string ServerName { get; set; }

        [JsonProperty("last_login_date")]
        public string LastLoginDate { get; set; }

    }
}
