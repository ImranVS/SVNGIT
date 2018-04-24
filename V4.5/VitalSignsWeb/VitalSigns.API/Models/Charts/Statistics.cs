using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class Statistics
    {
        [JsonProperty("device_id")]

        public int DeviceId { get; set; }

        [JsonProperty("stat_name")]
        public string StatName { get; set; }

        [JsonProperty("stat_value")]
        public double StatValue { get; set; }
    }
}
