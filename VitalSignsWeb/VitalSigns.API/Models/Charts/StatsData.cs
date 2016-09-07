using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class StatsData
    {
        [JsonProperty("device_id")]

        public string DeviceId { get; set; }

        [JsonProperty("stat_name")]
        public string StatName { get; set; }

        [JsonProperty("stat_value")]
        public double StatValue { get; set; }
    }
}
