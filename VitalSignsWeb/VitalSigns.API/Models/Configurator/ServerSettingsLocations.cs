using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServerSettingsLocations
    {


        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("type_id")]
        public string TypeId { get; set; }

        [JsonProperty("servers_list")]
        public string ServersList { get; set; }

       
    }
}
