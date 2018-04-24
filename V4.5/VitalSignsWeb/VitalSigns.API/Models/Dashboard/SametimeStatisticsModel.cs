using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class SametimeStatisticsModel
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        //[JsonProperty("totaln_way_chats")]
        //public int? TotalnWayChats { get; set; }

        //[JsonProperty("total2_way_chats")]
        //public int? Total2WayChats { get; set; }

        //[JsonProperty("peak_logins")]
        //public int? PeakLogins { get; set; }

        [JsonProperty("stat_name")]
        public string StatName { get; set; }

        [JsonProperty ("stat_value")]
        public double? StatValue { get; set; }
    }
}
