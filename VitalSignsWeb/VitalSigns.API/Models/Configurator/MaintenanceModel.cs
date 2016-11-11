using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MaintenanceModel
    {
        public MaintenanceModel()
        {
            DeviceList = new List<string>();
            KeyUsers = new List<string>();
        }
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("start_date")]
        public DateTime StartDate { get; set; }

        [JsonProperty("start_time")]
        public DateTime StartTime { get; set; }

        [JsonProperty("end_date")]
        public DateTime EndDate { get; set; }

        [JsonProperty("end_time")]
        public DateTime EndTime { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("maintenance_frequency")]
        public int MaintenanceFrequency { get; set; }

        [JsonProperty("maintenance_days_list")]
        public string MaintenanceDaysList { get; set; }

        [JsonProperty("maintain_type")]
        public string MaintainType { get; set; }

        [JsonProperty("maintain_type_value")]
        public string MaintainTypeValue { get; set; }

        [JsonProperty("continue_forever")]
        public bool ContinueForever { get; set; }

        [JsonProperty("device_list")]
        public List<string> DeviceList { get; set; }

        [JsonProperty("key_users")]
        public List<string> KeyUsers { get; set; }

        [JsonProperty("duration_type")]
        public string DurationType { get; set; }

        
    }
}
