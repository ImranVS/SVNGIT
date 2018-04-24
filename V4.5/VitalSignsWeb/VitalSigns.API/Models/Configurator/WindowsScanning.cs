using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class WindowsScanning
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("events")]
        public List<Events> Event { get; set; }

        [JsonProperty("windows_servers")]
        public List<string> WindowsServers { get; set; }

    }
    public class Events
    {
          
         
        [JsonProperty("alias_name")]
        public string AliasName { get; set; }          
         
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("event_id")]
        public int? EventId { get; set; }

        [JsonProperty("source")]
        public string Source { get; set; }

        [JsonProperty("event_name")]
        public string EventName { get; set; }

        [JsonProperty("event_level")]
        public string EventLevel { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }


    }
}
