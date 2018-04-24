using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ServersNewModel
    {


        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }


        [JsonProperty("ip_address")]
        public string IpAddress { get; set; }

      
        [JsonProperty("location_id")]
        public string LocationId { get; set; }

        [JsonProperty("business_hours_id")]
        public string BusinessHoursId { get; set; }

       
        [JsonProperty("monthly_operating_cost")]
        public int? MonthlyOperatingCost { get; set; }

        [JsonProperty("ideal_user_count")]
        public int? IdealUserCount { get; set; }

        [JsonProperty("category")]
        public string Category { get; set; }
    }
}
