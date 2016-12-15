using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class CommunityActivity
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("community")]
        public string Community { get; set; }

        [JsonProperty("object_name")]
        public string ObjectName { get; set; }

        [JsonProperty("object_value")]
        public int ObjectValue { get; set; }

        [JsonProperty("date_range")]
        public string DateRange { get; set; }
    }
}
