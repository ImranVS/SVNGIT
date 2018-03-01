using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class O365Licenses
    {
        [JsonProperty("license_type", NullValueHandling =NullValueHandling.Ignore)]
        public string LicenseType { get; set; }

        [JsonProperty("active_units", NullValueHandling = NullValueHandling.Ignore)]
        public int? ActiveUnits { get; set; }

        [JsonProperty("warning_units", NullValueHandling = NullValueHandling.Ignore)]
        public int? WarningUnits { get; set; }

        [JsonProperty("locked_out_units", NullValueHandling = NullValueHandling.Ignore)]
        public int? LockedOutUnits { get; set; }

        [JsonProperty("suspended_units", NullValueHandling = NullValueHandling.Ignore)]
        public int? SuspendedUnits { get; set; }
    }
}
