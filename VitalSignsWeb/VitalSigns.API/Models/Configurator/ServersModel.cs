using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServersModel
    {


        [JsonProperty("device_name")]
        public string ServerName { get; set; }

        [JsonProperty("device_type")]
        public string ServerType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("server_id")]
        public List<string> ServerId { get; set; }


    }
}
