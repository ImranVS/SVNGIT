using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VitalSigns.API.Models.Configurator
{
    public class LicenseInformation
    {
        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("license_cost")]
        public double? LicenseCost { get; set; }    
    }
}
