using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ExchangeMailprobe
    {
        [JsonProperty("source_server")]
        public string SourceServer { get; set; }

        [JsonProperty("destination_server")]
        public string DestinationServer { get; set; }

        [JsonProperty("latency")]
        public double? Latency { get; set; }
    }
}
