using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    ///<Author>Sowjanya Korumilli</Author>
    /// <summary>
    /// Status Summary
    /// </summary>
    public class StatusSummary
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("ok")]
        public int? Ok { get; set; }

        [JsonProperty("not_responding")]
        public int? NotResponding { get; set; }

        [JsonProperty("issue")]
        public int? Issue { get; set; }

        [JsonProperty("maintenance")]
        public int? Maintenance { get; set; }
    }
}
