using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Request
{
    public class StatisticsRequest
    {

        [JsonProperty("stat_name")]
        public string StatName { get; set; }

        [JsonProperty("operation")]
        public string Operation { get; set; }
    }
}
