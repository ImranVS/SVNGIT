using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class UserAdoption
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("object_name")]
        public string ObjectName { get; set; }

        [JsonProperty("object_value")]
        public int ObjectValue { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }
    }

    public class UserAdoptionPivot
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("object_values")]
        public List<int> ObjectValues { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
