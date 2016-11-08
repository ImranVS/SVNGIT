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

        [JsonProperty("notification")]
        public string Notification { get; set; }

        [JsonProperty("os")]
        public string OperatingSystem { get; set; }

        [JsonProperty("last_sync_time")]
        public DateTime? LastSyncTime { get; set; }

        [JsonProperty("access")]
        public string Access { get; set; }


        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("is_selected")]
        public bool IsSelected { get; set; }
    }
}
