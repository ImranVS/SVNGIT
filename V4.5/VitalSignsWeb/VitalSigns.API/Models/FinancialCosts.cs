using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class FinancialCosts
    {
        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("user_count")]
        public double UserCount { get; set; }

        [JsonProperty("monthly_operating_cost")]
        public double MonthlyOperatingCost { get; set; }

        [JsonProperty("cost_per_day")]
        public double CostPerDay { get; set; }

        [JsonProperty("cost_per_user")]
        public double CostPerUser { get; set; }

        [JsonProperty("date")]
        public System.DateTime DateTime { get; set; }
        
    }
}