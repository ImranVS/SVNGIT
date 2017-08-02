using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models
{
    public class MobileUserDevice
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("user_name")]
        public string UserName { get; set; }

        [JsonProperty("device")]
        public string Device { get; set; }

        [JsonProperty("device_name")]
        public string DeviceName { get; set; }

        [JsonProperty("notification")]
        public string Notification { get; set; }

        [JsonProperty("os_type")]
        public string OperatingSystem { get; set; }

        [JsonProperty("last_sync_time")]
        public DateTime? LastSyncTime { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }


        [JsonProperty("device_id")]
        public string DeviceId { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }

        [JsonProperty("threshold_sync_time")]
        public int? ThresholdSyncTime { get; set; }

        [JsonProperty("os")]
        public string OS { get; set; }

        [JsonProperty("last_sync_ago")]
        public double LastSyncAgo { get; set; }

        [JsonProperty("device_user_count")]
        public int DeviceUserCount { get; set; }
    }
}
