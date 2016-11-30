using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VitalSigns.API.Models.Configurator
{
    public class ServerEventsModel
    {
        [JsonProperty("notification_name")]
        public string NotificationName { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("send_to")]
        public string SendTo { get; set; }

        [JsonProperty("copy_to")]
        public string CopyTo { get; set; }

        [JsonProperty("blind_copy_to")]
        public string BlindCopyTo { get; set; }

        [JsonProperty("event_type")]
        public string EventType { get; set; }

        [JsonProperty("days")]
        public string[] Days { get; set; }
    }
}
