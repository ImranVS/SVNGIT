using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class Outage
    {
        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("date_time_down")]
        public string DateTimeDown { get; set; }

        [JsonProperty("date_time_up")]
        public string DateTimeUp { get; set; }

    }
}
