using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace VitalSigns.API.Models
{
    ///<Author>Sowmya Pathuri</Author>
    /// <summary>
    /// Status Model
    /// </summary>
    public class StatusBox
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("last_scan")]
        public DateTime? LastScan { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("icon")]
        public string Icon { get; set; }
    }
}
