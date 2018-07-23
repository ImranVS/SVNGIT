using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class ConnectionsBreakdown
    {

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("num_of_logins")]
        public string NumOfLogins { get; set; }

        [JsonProperty("percent_of_active_users")]
        public double ActiveUsersPercentage { get; set; }

        [JsonProperty("types")]
        public List<ConnectionsBreakdownType> Types { get; set; }
    }

    public class ConnectionsBreakdownType
    {

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("base_type")]
        public string BaseType { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("new_count")]
        public int NewCount { get; set; }

        [JsonProperty("is_is_community")]
        public bool IsInCommunity { get; set; }
        
    }
}
