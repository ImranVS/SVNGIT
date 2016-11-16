using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class SummaryDataModel
    {
        [JsonProperty("device_id")]
        public string DeviceID { get; set; }

        [JsonProperty("stat_name")]
        public string StatName { get; set; }

        [JsonProperty("value")]
        public double? Value { get; set; }
    }
}
