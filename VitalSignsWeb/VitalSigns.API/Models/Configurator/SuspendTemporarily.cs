using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class SuspendTemporarilyModel
    {
      
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }


        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }
    }
}
