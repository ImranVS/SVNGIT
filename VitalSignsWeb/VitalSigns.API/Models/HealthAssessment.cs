using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class HealthAssessment
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("last_scan")]
        public DateTime? LastScan { get; set; }


        [JsonProperty("test_name")]
        public string TestName { get; set; }

        [JsonProperty("result")]
        public string Result { get; set; }

        [JsonProperty("details")]
        public string Details { get; set; }
    }
}
