using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class ServerType
    {
        [JsonProperty("server_type")]
        public string ServerTypeName { get; set; }
        [JsonProperty("icon")]
        public string Icon { get; set; }
        [JsonProperty("tabs")]
        public List<Tab> Tabs { get; set; }
    }

    public class Tab
    {        
        [JsonProperty("type")]
        public string Type { get; set; }
      
        [JsonProperty("title")]
        public string Title { get; set; }
      
        [JsonProperty("component")]
        public string Component { get; set; }
       
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("secondary_role")]
        public string SecondaryRole { get; set; }
        [JsonProperty("active")]
        public bool Active { get; set; }
    }
}
