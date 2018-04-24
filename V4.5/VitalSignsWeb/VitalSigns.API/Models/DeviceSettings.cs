using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models
{
    public class DeviceSettings
    {
        [JsonProperty("setting")]
        public object Setting { get; set; }
        [JsonProperty("value")]
        public object Value { get; set; }
        [JsonProperty("devices")]
        public object Devices { get; set; }
    }
}
