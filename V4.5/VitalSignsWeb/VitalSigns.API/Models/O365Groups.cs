using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class O365Groups
    {
        [JsonProperty("device_name", NullValueHandling =NullValueHandling.Ignore)]
        public string DeviceName { get; set; }

        [JsonProperty("group_name", NullValueHandling = NullValueHandling.Ignore)]
        public string GroupName { get; set; }

        [JsonProperty("group_type", NullValueHandling = NullValueHandling.Ignore)]
        public string GroupType { get; set; }

        [JsonProperty("members", NullValueHandling = NullValueHandling.Ignore)]
        public string Members { get; set; }

      

        
    }
}
