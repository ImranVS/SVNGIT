using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Dashboard
{
    public class SametimeStatisticsModel
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("totaln_way_chats")]
        public int? TotalnWayChats { get; set; }

        [JsonProperty("total2_way_chats")]
        public int? Total2WayChats { get; set; }

        [JsonProperty("peak_logins")]
        public int? PeakLogins { get; set; }

    }
}
