using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Charts
{
    public class IbmTopTagConnection
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty ("ranking")]
        public string Ranking { get; set; }

        [JsonProperty ("type")]
        public string Type { get; set; }

        [JsonProperty("usage_count")]
        public string UsageCount { get; set; }

        
    }
}
