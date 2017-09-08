using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class Office365Node
    {

        [JsonProperty("node_id")]
        public string NodeId { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }
    
        [JsonProperty("host_name")]
        public string HostName { get; set; }
        
    }
}
